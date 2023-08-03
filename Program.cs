using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace usearch
{
    using usearch_index_t = System.IntPtr;
    using usearch_label_t = System.UIntPtr;
    using usearch_distance_t = System.Single;
    using usearch_error_t = System.String;

    internal static unsafe class Program
    {
        public delegate usearch_distance_t usearch_metric_t(void* a, void* b);

        public enum usearch_metric_kind_t
        {
            usearch_metric_ip_k = 0,
            usearch_metric_l2sq_k,
            usearch_metric_cos_k,
            usearch_metric_haversine_k,
            usearch_metric_pearson_k,
            usearch_metric_jaccard_k,
            usearch_metric_hamming_k,
            usearch_metric_tanimoto_k,
            usearch_metric_sorensen_k,
            usearch_metric_unknown_k,
        }

        public enum usearch_scalar_kind_t
        {
            usearch_scalar_f32_k = 0,
            usearch_scalar_f64_k,
            usearch_scalar_f16_k,
            usearch_scalar_f8_k,
            usearch_scalar_b1_k,
            usearch_scalar_unknown_k,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct usearch_init_options_t
        {
            public usearch_metric_kind_t metric_kind;
            public usearch_metric_t? metric;

            public usearch_scalar_kind_t quantization;
            public nuint dimensions;
            public nuint connectivity;
            public nuint expansion_add;
            public nuint expansion_search;
        }

        [DllImport("libusearch", EntryPoint = "usearch_init")]
        private static extern usearch_index_t _usearch_init(ref usearch_init_options_t options, out nint error);

        public static usearch_index_t usearch_init(ref usearch_init_options_t options, out usearch_error_t? error)
        {
            var result = _usearch_init(ref options, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_free")]
        private static extern void _usearch_free(usearch_index_t index, out nint error);

        public static void usearch_free(usearch_index_t index, out usearch_error_t? error)
        {
            _usearch_free(index, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("libusearch", EntryPoint = "usearch_save")]
        private static extern void _usearch_save(usearch_index_t index, string path, out nint error);

        public static void usearch_save(usearch_index_t index, string path, out usearch_error_t? error)
        {
            _usearch_save(index, path, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("libusearch", EntryPoint = "usearch_load")]
        private static extern void _usearch_load(usearch_index_t index, string path, out nint error);

        public static void usearch_load(usearch_index_t index, string path, out usearch_error_t? error)
        {
            _usearch_load(index, path, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("libusearch", EntryPoint = "usearch_view")]
        private static extern void _usearch_view(usearch_index_t index, string path, out nint error);

        public static void usearch_view(usearch_index_t index, string path, out usearch_error_t? error)
        {
            _usearch_view(index, path, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("libusearch", EntryPoint = "usearch_size")]
        private static extern nuint _usearch_size(usearch_index_t index, out nint error);

        public static nuint usearch_size(usearch_index_t index, out usearch_error_t? error)
        {
            var result = _usearch_size(index, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_capacity")]
        private static extern nuint _usearch_capacity(usearch_index_t index, out nint error);

        public static nuint usearch_capacity(usearch_index_t index, out usearch_error_t? error)
        {
            var result = _usearch_capacity(index, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_dimensions")]
        private static extern nuint _usearch_dimensions(usearch_index_t index, out nint error);

        public static nuint usearch_dimensions(usearch_index_t index, out usearch_error_t? error)
        {
            var result = _usearch_dimensions(index, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_connectivity")]
        private static extern nuint _usearch_connectivity(usearch_index_t index, out nint error);

        public static nuint usearch_connectivity(usearch_index_t index, out usearch_error_t? error)
        {
            var result = _usearch_connectivity(index, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_reserve")]
        private static extern void _usearch_reserve(usearch_index_t index, nuint capacity, out nint error);

        public static void usearch_reserve(usearch_index_t index, nuint capacity, out usearch_error_t? error)
        {
            _usearch_reserve(index, capacity, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("libusearch", EntryPoint = "usearch_add")]
        private static extern void _usearch_add(usearch_index_t index, usearch_label_t label, void* vector, usearch_scalar_kind_t vector_kind, out nint error);

        public static void usearch_add(usearch_index_t index, usearch_label_t label, void* vector, usearch_scalar_kind_t vector_kind, out usearch_error_t? error)
        {
            _usearch_add(index, label, vector, vector_kind, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("libusearch", EntryPoint = "usearch_contains")]
        private static extern bool _usearch_contains(usearch_index_t index, usearch_label_t label, out nint error);

        public static bool usearch_contains(usearch_index_t index, usearch_label_t label, out usearch_error_t? error)
        {
            var result = _usearch_contains(index, label, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_search")]
        private static extern nuint _usearch_search(
            usearch_index_t index,
            void* query_vector,
            usearch_scalar_kind_t query_kind,
            nuint results_limit,
            usearch_label_t[] found_labels,
            usearch_distance_t[] found_distances,
            out nint error
        );

        public static nuint usearch_search(
            usearch_index_t index,
            void* query_vector,
            usearch_scalar_kind_t query_kind,
            nuint results_limit,
            usearch_label_t[] found_labels,
            usearch_distance_t[] found_distances,
            out usearch_error_t? error
        )
        {
            var result = _usearch_search(index, query_vector, query_kind, results_limit, found_labels, found_distances, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_get")]
        private static extern bool _usearch_get(usearch_index_t index, usearch_label_t label, nint vector, usearch_scalar_kind_t vector_kind, out nint error);

        public static bool usearch_get(usearch_index_t index, usearch_label_t label, nint vector, usearch_scalar_kind_t vector_kind, out usearch_error_t? error)
        {
            var result = _usearch_get(index, label, vector, vector_kind, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
            return result;
        }

        [DllImport("libusearch", EntryPoint = "usearch_remove")]
        private static extern void _usearch_remove(usearch_index_t index, usearch_label_t label, out nint error);

        public static void usearch_remove(usearch_index_t index, usearch_label_t label, out usearch_error_t? error)
        {
            _usearch_remove(index, label, out var ptr);
            error = Marshal.PtrToStringAnsi(ptr);
        }

        static usearch_init_options_t create_options(nuint vector_dimension)
        {
            usearch_init_options_t opts;
            opts.connectivity = 2;
            opts.dimensions = vector_dimension;
            opts.expansion_add = 40;
            opts.expansion_search = 16;
            opts.metric_kind = usearch_metric_kind_t.usearch_metric_cos_k;
            opts.metric = null;
            opts.quantization = usearch_scalar_kind_t.usearch_scalar_f32_k;
            return opts;
        }

        static void test_init(nuint vectors_count, nuint vector_dimension)
        {
            Console.Write("Test: Index Initialization...\n");

            usearch_init_options_t opts = create_options(vector_dimension);
            usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
            Debug.Assert(error == null, error);
            usearch_free(idx, out error);
            Debug.Assert(error == null, error);

            idx = usearch_init(ref opts, out error);
            Debug.Assert(error == null, error);

            Debug.Assert(usearch_size(idx, out error) == 0, error);
            Debug.Assert(usearch_capacity(idx, out error) == 0, error);
            Debug.Assert(usearch_dimensions(idx, out error) == vector_dimension, error);
            Debug.Assert(usearch_connectivity(idx, out error) == opts.connectivity, error);

            usearch_reserve(idx, vectors_count, out error);
            Debug.Assert(error == null, error);
            Debug.Assert(usearch_size(idx, out error) == 0, error);
            Debug.Assert(usearch_capacity(idx, out error) == vectors_count, error);
            Debug.Assert(usearch_dimensions(idx, out error) == vector_dimension, error);
            Debug.Assert(usearch_connectivity(idx, out error) == opts.connectivity, error);

            usearch_free(idx, out error);
            Debug.Assert(error == null, error);

            Console.Write("Test: Index Initialization - PASSED\n");
        }

        static void test_add_vector<T>(nuint vectors_count, nuint vector_dimension, T* data, usearch_scalar_kind_t vector_kind)
        {
            Console.Write("Test: Add Vector...\n");

            usearch_init_options_t opts = create_options(vector_dimension);
            usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
            usearch_reserve(idx, vectors_count, out error);

            // Add vectors
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                usearch_add(idx, label, data + i * vector_dimension, vector_kind, out error);
                Debug.Assert(error == null, error);
            }

            Debug.Assert(usearch_size(idx, out error) == vectors_count, error);
            Debug.Assert(usearch_capacity(idx, out error) == vectors_count, error);

            // Check vectors in the index
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                Debug.Assert(usearch_contains(idx, label, out error), error);
            }
            Debug.Assert(!usearch_contains(idx, unchecked((nuint)(-1)), out error), error); // Non existing label

            usearch_free(idx, out error);
            Console.Write("Test: Add Vector - PASSED\n");
        }

        static void test_find_vector<T>(nuint vectors_count, nuint vector_dimension, T* data, usearch_scalar_kind_t vector_kind)
        {
            Console.Write("Test: Find Vector...\n");

            usearch_init_options_t opts = create_options(vector_dimension);
            usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
            usearch_reserve(idx, vectors_count, out error);

            // Create result buffers
            nuint results_count = 10;
            usearch_label_t[] labels = new usearch_label_t[results_count];
            float[] distances = new float[results_count];
            Debug.Assert(labels != null && distances != null, "Failed to allocate memory");

            // Add vectors
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                usearch_add(idx, label, data + i * vector_dimension, vector_kind, out error);
                Debug.Assert(error == null, error);
            }

            // Find the vectors
            for (nuint i = 0; i < vectors_count; i++)
            {
                void* query_vector = data + i * vector_dimension;
                nuint found_count = usearch_search(idx, query_vector, vector_kind, results_count, labels, distances, out error);
                Debug.Assert(error == null, error);
                Debug.Assert((found_count = results_count) > 0, "Vector is missing");
            }

            usearch_free(idx, out error);
            Console.Write("Test: Find Vector - PASSED\n");
        }

        static void test_remove_vector<T>(nuint vectors_count, nuint vector_dimension, T* data, usearch_scalar_kind_t vector_kind)
        {
            Console.Write("Test: Remove Vector...\n");

            usearch_init_options_t opts = create_options(vector_dimension);
            usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
            usearch_reserve(idx, vectors_count, out error);

            // Add vectors
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                usearch_add(idx, label, data + i * vector_dimension, vector_kind, out error);
                Debug.Assert(error == null, error);
            }

            // Remove the vectors
            for (nuint i = 0; i < vectors_count; i++)
            {
                usearch_label_t label = i;
                usearch_remove(idx, label, out error);
                Debug.Assert(!String.IsNullOrEmpty(error), "Currently, Remove is not supported");
            }

            usearch_free(idx, out error);
            Console.Write("Test: Remove Vector - PASSED\n");
        }

        static void test_save_load<T>(nuint vectors_count, nuint vector_dimension, T* data, usearch_scalar_kind_t vector_kind)
        {
            Console.Write("Test: Save/Load...\n");

            usearch_init_options_t opts = create_options(vector_dimension);
            usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
            usearch_reserve(idx, vectors_count, out error);

            // Add vectors
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                usearch_add(idx, label, data + i * vector_dimension, vector_kind, out error);
                Debug.Assert(error == null, error);
            }

            // Save and free the index
            usearch_save(idx, "usearch_index.bin", out error);
            Debug.Assert(error == null, error);
            usearch_free(idx, out error);
            Debug.Assert(error == null, error);

            // Reinit
            idx = usearch_init(ref opts, out error);
            Debug.Assert(error == null, error);
            Debug.Assert(usearch_size(idx, out error) == 0, error);

            // Load
            usearch_load(idx, "usearch_index.bin", out error);
            Debug.Assert(error == null, error);
            Debug.Assert(usearch_size(idx, out error) == vectors_count, error);
            Debug.Assert(usearch_capacity(idx, out error) == vectors_count, error);
            Debug.Assert(usearch_dimensions(idx, out error) == vector_dimension, error);
            Debug.Assert(usearch_connectivity(idx, out error) == opts.connectivity, error);

            // Check vectors in the index
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                Debug.Assert(usearch_contains(idx, label, out error), error);
            }

            usearch_free(idx, out error);
            Console.Write("Test: Save/Load - PASSED\n");
        }

        static void test_view<T>(nuint vectors_count, nuint vector_dimension, T* data, usearch_scalar_kind_t vector_kind)
        {
            Console.Write("Test: View...\n");

            usearch_init_options_t opts = create_options(vector_dimension);
            usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
            usearch_reserve(idx, vectors_count, out error);

            // Add vectors
            for (nuint i = 0; i < vectors_count; ++i)
            {
                usearch_label_t label = i;
                usearch_add(idx, label, data + i * vector_dimension, vector_kind, out error);
                Debug.Assert(error == null, error);
            }

            // Save and free the index
            usearch_save(idx, "usearch_index.bin", out error);
            Debug.Assert(error == null, error);
            usearch_free(idx, out error);
            Debug.Assert(error == null, error);

            // Reinit
            idx = usearch_init(ref opts, out error);
            Debug.Assert(error == null, error);

            // View
            usearch_view(idx, "usearch_index.bin", out error);
            Debug.Assert(error == null, error);

            usearch_free(idx, out error);
            Console.Write("Test: View - PASSED\n");
        }

        static void Main(string[] args)
        {
            var data = JsonSerializer.Deserialize<float[][]>(File.ReadAllText("sample.fvecs"));

            nuint vectors_count = (nuint)(data?.Length ?? 0);
            nuint vector_dimension = (nuint)(data?[0].Length ?? 0);
            usearch_scalar_kind_t vector_kind = usearch_scalar_kind_t.usearch_scalar_f32_k;

            fixed (float* ptr = data?[0])
            {
                // Test
                test_init(vectors_count, vector_dimension);
                test_add_vector(vectors_count, vector_dimension, ptr, vector_kind);
                test_find_vector(vectors_count, vector_dimension, ptr, vector_kind);
                test_remove_vector(vectors_count, vector_dimension, ptr, vector_kind);
                test_save_load(vectors_count, vector_dimension, ptr, vector_kind);
                test_view(vectors_count, vector_dimension, ptr, vector_kind);

                //{
                //    usearch_init_options_t opts = create_options(vector_dimension);
                //    usearch_index_t idx = usearch_init(ref opts, out usearch_error_t? error);
                //    usearch_reserve(idx, vectors_count, out error);

                //    for (usearch_label_t label = 0; label < vectors_count; ++label)
                //        usearch_add(idx, label, ptr + label * vector_dimension, vector_kind, out error);

                //    var results_count = vectors_count - 1;
                //    usearch_label_t[] labels = new usearch_label_t[results_count];
                //    var distances = new float[results_count];

                //    var query_vector = ptr + results_count * vector_dimension;
                //    var found_count = usearch_search(idx, query_vector, vector_kind, results_count, labels, distances, out error);

                //    for (nuint i = 0; i < found_count; ++i)
                //    {
                //        Console.WriteLine($"{i}: {distances[i]}");
                //    }
                //}
            }
        }
    }
}
