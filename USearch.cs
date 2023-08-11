using static LibUSearch.Interop;

namespace USearch
{
    using usearch_index_t = System.IntPtr;
    using usearch_key_t = System.UInt64;
    using usearch_distance_t = System.Single;
    using usearch_error_t = System.String;

    public class Index<T> : IDisposable where T : struct
    {
        private usearch_index_t _handle;
        private bool _disposed;
        private usearch_init_options_t _options;
        private string? _path = null;

        public Index(ulong dimension)
        {
            _options = new()
            {
                connectivity = 2,
                dimensions = (nuint)dimension,
                expansion_add = 64,
                expansion_search = 16,
                metric_kind = usearch_metric_kind_t.usearch_metric_cos_k,
                metric = null,
                quantization = usearch_scalar_kind_t.usearch_scalar_f32_k,
            };

            _handle = usearch_init(ref _options, out var error);
            _ThrowIfError(error);
        }

        public Index(int dimension) : this((ulong)dimension) { }

        public Index(string path, bool view = false)
        {
            _options = new();

            usearch_error_t? error;
            _handle = usearch_init(ref _options, out error);
            _ThrowIfError(error);

            if (!view)
                usearch_load(_handle, path, out error);
            else
                usearch_view(_handle, path, out error);

            _ThrowIfError(error);

            _path = path;

            _options.connectivity = (nuint)this.Connectivity;
            _options.dimensions = (nuint)this.Dimension;
            _options.quantization = usearch_scalar_kind_t.usearch_scalar_f32_k;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Managed
                    usearch_free(_handle, out var error);
                    _ThrowIfError(error);
                }

                // Unmanaged

                _disposed = true;
            }
        }

        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Index()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void _ThrowIfError(usearch_error_t? error)
        {
            if (error != null)
                throw new Exception(error);
        }

        public int Dimension
        {
            get
            {
                var dimension = usearch_dimensions(_handle, out var error);
                _ThrowIfError(error);
                return (int)dimension;
            }
        }

        public int Size
        {
            get
            {
                var size = usearch_size(_handle, out var error);
                _ThrowIfError(error);
                return (int)size;
            }
        }

        public int Capacity
        {
            get
            {
                var capacity = usearch_capacity(_handle, out var error);
                _ThrowIfError(error);
                return (int)capacity;
            }
        }

        public int Connectivity
        {
            get
            {
                var connectivity = usearch_connectivity(_handle, out var error);
                _ThrowIfError(error);
                return (int)connectivity;
            }
        }

        public void Save(string? path = null)
        {
            if (path == null && _path == null)
                throw new InvalidOperationException("No path specified.");

            usearch_save(_handle, path != null ? path : _path ?? String.Empty, out var error);
            _ThrowIfError(error);
        }

        public void Reserve(ulong capacity)
        {
            usearch_reserve(_handle, capacity, out var error);
            _ThrowIfError(error);
        }

        public void Reserve(int capacity) => Reserve((ulong)capacity);

        public void Add(T[] vector)
        {
            usearch_add(_handle, (usearch_key_t)this.Size, vector, out var error);
            _ThrowIfError(error);
        }

        public void Add(T[][] vectors)
        {
            for (var i = 0; i < vectors.Length; i++)
                Add(vectors[i]);
        }

        public void Add(usearch_key_t key, T[] vector)
        {
            usearch_add(_handle, key, vector, out var error);
            _ThrowIfError(error);
        }

        public void Add(usearch_key_t[] keys, T[][] vectors)
        {
            for (var i = 0; i < keys.Length; i++)
                Add(keys[i], vectors[i]);
        }

        public void Add(IDictionary<usearch_key_t, T[]> vectors)
        {
            foreach (var vector in vectors)
                Add(vector.Key, vector.Value);
        }

        public bool Get(usearch_key_t key, out T[]? vector)
        {
            var result = usearch_get(_handle, key, out vector, out var error);
            _ThrowIfError(error);
            return result;
        }

        public void Remove(usearch_key_t key)
        {
            usearch_remove(_handle, key, out var error);
            _ThrowIfError(error);
        }

        public bool Contains(usearch_key_t key)
        {
            var result = usearch_contains(_handle, key, out var error);
            _ThrowIfError(error);
            return result;
        }

        public Dictionary<usearch_key_t, usearch_distance_t> Search(T[] vector, int limit = -1)
        {
            if (limit == 0)
                return new();

            var size = this.Size;
            if (limit < 0 || limit >= size)
                limit = size;

            _ = usearch_search(_handle, vector, (ulong)limit, out var results, out var error);
            _ThrowIfError(error);
            return results;
        }
    }
}
