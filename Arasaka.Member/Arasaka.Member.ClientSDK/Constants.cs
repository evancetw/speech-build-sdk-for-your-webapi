namespace Arasaka.Member.ClientSDK
{
    public static class Constants
    {
        public enum MemberState : byte
        {
            FSM_BEGIN = byte.MinValue,

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

            FSM_END = byte.MaxValue,
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