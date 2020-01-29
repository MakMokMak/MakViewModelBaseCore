using System.Windows;

namespace MakCraft.Utils
{
    /// <summary>
    /// 表示モニター名と相対座標を表します。
    /// </summary>
    public class RelativePointInfo
    {
        /// <summary>
        /// 新しい表示モニター名と相対座標を格納するクラス。
        /// </summary>
        /// <param name="point"></param>
        /// <param name="monitorName"></param>
        public RelativePointInfo(Point point, string monitorName)
        {
            Point = point;
            MonitorName = monitorName;
        }

        /// <summary>
        /// 相対座標を取得します。
        /// </summary>
        public Point Point { get; }

        /// <summary>
        /// モニター名を取得します。
        /// </summary>
        public string MonitorName { get; }
    }
}
