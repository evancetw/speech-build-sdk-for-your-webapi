namespace Arasaka.Member.Api
{
    internal static class Constants
    {
        public enum MemberState
        {
            /// <summary>
            /// 未認證
            /// </summary>
            Unverified,

            /// <summary>
            /// 已認證
            /// </summary>
            Verified,

            /// <summary>
            /// (權限) 受限
            /// </summary>
            Restricted,

            /// <summary>
            /// 禁止
            /// </summary>
            Banned,
        }

        /// <summary>
        /// 社會性別
        /// </summary>
        public enum Gender
        {
            Female,
            Male,
            NonBinary,
        }
    }
}
