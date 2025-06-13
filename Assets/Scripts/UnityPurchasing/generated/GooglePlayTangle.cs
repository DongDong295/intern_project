// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("ectIa3lET0BjzwHPvkRISEhMSUrLSEZJectIQ0vLSEhJ9XOT3AQ+5TkcO8WeP33AoQ635GsFb1mQQvpyEAyI+Cqyj+dk13Oa4k79si4rljOysBncOHB6hD69QtwQ/BmvaMItovm9YCTavexxNni1rk8v1HsYfRSwb0+oYCM1O9eHXtlanZ7RsyJ2G7BFCQ3pEYPWWrV+YYye1sL0Ia/cPRI703HZX8QUHIQfrZ+Bb0eMsUOidBp1/mdXTzV8HnQ73umPUHiauWOFZd1kJPihA9wmLG3nioFoMA+8G6TU64hsDwa2vY4cV9Z6D1V2yTTn4aHGF4MaRUtD2bGUzQ8fPCfW9XfenNo8IuJ+aOmnMekmPl8j8iTHLqNtH2w97NRO5EtKSElI");
        private static int[] order = new int[] { 0,1,12,8,6,13,10,9,11,12,12,12,13,13,14 };
        private static int key = 73;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
