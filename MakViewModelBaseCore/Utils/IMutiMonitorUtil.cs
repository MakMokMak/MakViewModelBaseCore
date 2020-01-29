using System.Windows;

namespace MakCraft.Utils
{
    /// <summary>
    /// マルチモニター ユーティリティのインターフェイスです。
    /// </summary>
    public interface IMutiMonitorUtil
    {
        /// <summary>
        /// 表示可能座標の確認で利用するマージン値を取得・設定します。
        /// 値の設定は初回のみ可能です。
        /// </summary>
        double Margin { get; set; }

        /// <summary>
        /// スクリーン情報をリフレッシュします。
        /// </summary>
        /// <returns>情報の更新があった場合は true を返します。</returns>
        bool Refresh();

        /// <summary>
        /// プライマリモニターとなっているモニターの名前(デバイス名)を返します。
        /// </summary>
        /// <returns>モニターの名前(デバイス名)</returns>
        string GetPrimaryMonitorName();

        /// <summary>
        /// モニター名(デバイス名)の一致するモニターの表示エリアを返します。
        /// </summary>
        /// <param name="monitorName">モニター名(デバイス名)</param>
        /// <returns>表示エリアを示す Rect 構造体</returns>
        Rect GetWorkingArea(string monitorName);

        /// <summary>
        /// 与えられた Rect 構造体の座標がマルチモニターの表示範囲内に収まっているかの真偽値を返します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>真偽値(真であれば表示範囲内)</returns>
        bool IsInRange(Rect target);

        /// <summary>
        /// 与えられた Rect 構造体の座標が表示範囲内に収まっているモニター名を返します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>モニター名(座標がすべてのモニターの表示範囲から外れている場合は null)</returns>
        string GetInRangeMonitorName(Rect target);

        /// <summary>
        /// 与えられた Rect 構造体の座標が表示範囲内に収まっているモニター名を返します。
        /// このメソッドはマージン値を考慮せずに判定します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>モニター名(座標がすべてのモニターの表示範囲から外れている場合は null)</returns>
        string GetInRangeMonitorNameExcludingMargin(Rect target);

        /// <summary>
        /// 与えられた Rect 構造体の座標の中心が位置するモニター名を返します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>モニター名(座標の中心がすべてのモニターの表示範囲から外れている場合は null)</returns>
        string GetMoniterNameCenterPosition(Rect target);

        /// <summary>
        /// モニター名とモニター上の相対座標から表示座標を得ます。
        /// モニター名が存在しない場合は、プライマリモニター上の座標を返します。
        /// 得られた座標が表示可能域を外れていた場合は、画面中央の座標を返します。
        /// </summary>
        /// <param name="target">モニター上の相対座標と大きさを保持する Rect 構造体</param>
        /// <param name="monitorName">表示するモニター名</param>
        /// <returns>ウィンドウを表示する座標</returns>
        Point GetDisplayPosition(Rect target, string monitorName);

        /// <summary>
        /// 与えられた Rect 構造体の座標から相対座標情報を得ます。
        /// </summary>
        /// <param name="target">Window の座標を格納した Rect 構造体</param>
        /// <returns>相対座標情報を格納した RelativePointInfo クラスのインスタンス</returns>
        RelativePointInfo GetRelativePointInfo(Rect target);

        /// <summary>
        /// 与えられた Rect 構造体の座標とモニター名から相対座標情報を得ます。
        /// </summary>
        /// <param name="target">Window の座標を格納した Rect 構造体</param>
        /// <param name="monitorName">相対座標を得るモニターの名前</param>
        /// <returns>相対座標を格納した Point 構造体</returns>
        Point GetRelativePoint(Rect target, string monitorName);
    }
}
