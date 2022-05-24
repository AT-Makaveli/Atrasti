using System;
using System.Text;
using NTypewriter.Editor.Config;

namespace Atrasti.TypeWriter
{
    public class CustomConfiguration : EditorConfig
    {
        public static string ToSnakeCase(string text)
        {
            if(text == null) {
                throw new ArgumentNullException(nameof(text));
            }
            if(text.Length < 2) {
                return text;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToUpperInvariant(text[0]));
            for(int i = 1; i < text.Length; ++i) {
                char c = text[i];
                if(char.IsUpper(c)) {
                    sb.Append('_');
                    sb.Append(char.ToUpperInvariant(c));
                } else {
                    sb.Append(char.ToUpperInvariant(c));
                }
            }
            return sb.ToString();
        }
    }
}