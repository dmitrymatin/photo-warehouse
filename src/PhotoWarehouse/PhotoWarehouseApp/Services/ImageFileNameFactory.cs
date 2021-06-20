using System;

namespace PhotoWarehouseApp.Services
{
    public static class ImageFileNameFactory
    {
        public static string GenerateFileName(string initialFileName, string fileNameToProcess)
        {
            fileNameToProcess = FileService.EnsureCorrectFileName(fileNameToProcess);
            string extension = FileService.GetFileExtension(fileNameToProcess);

            string initialFileNameWithoutCount = "";
            int count = 0;
            if (initialFileName is null)
            {
                initialFileNameWithoutCount = Guid.NewGuid().ToString();
            }
            else
            {
                int bracketStart = initialFileName.LastIndexOf('(');
                int bracketEnd = initialFileName.LastIndexOf(')');

                if (bracketStart != -1 || bracketEnd != -1)
                {
                    initialFileNameWithoutCount = GetFileNameWithoutCount(initialFileName, bracketStart);
                    count = GetCount(initialFileName, bracketStart, bracketEnd);

                    if (count == -1)
                        throw new InvalidOperationException($"file name generation for string has failed {fileNameToProcess}");
                }
            }

            return $"{initialFileNameWithoutCount}({count + 1}){extension}";
        }

        private static string GetFileNameWithoutCount(string filename, int bracketStart)
        {
            return filename.Substring(0, bracketStart);
        }

        private static int GetCount(string fileName, int bracketStart, int bracketEnd)
        {
            string countString = fileName.Substring(bracketStart + 1, bracketEnd - bracketStart - 1);
            if (int.TryParse(countString, out int count))
            {
                return count;
            }

            return -1;
        }
    }
}
