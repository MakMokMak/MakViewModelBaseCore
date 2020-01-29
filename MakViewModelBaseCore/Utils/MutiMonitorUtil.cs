using System;
using System.Linq;
using System.Windows;
using FORM = System.Windows.Forms;

namespace MakCraft.Utils
{
    /// <summary>
    /// マルチモニターへの表示に関するユーティリティです。
    /// </summary>
    public class MutiMonitorUtil : IMutiMonitorUtil
    {
        static MutiMonitorUtil() { } // コンパイラによる beforefieldinit フラグの付加を抑止する
        private MutiMonitorUtil() { } //外部からのインスタンス生成を抑止

        private static readonly Lazy<MutiMonitorUtil> _instance = new Lazy<MutiMonitorUtil>(() =>
        {
            var instance = new MutiMonitorUtil();
            instance.Refresh();
            return instance;
        });

        private readonly object _lockScreens = new object();
        private static FORM::Screen[] _screens;

        /// <summary>
        /// MutiMonitorUtil のインスタンスを取得します。
        /// </summary>
        public static IMutiMonitorUtil Instance => _instance.Value;

        private double? _margin;
        /// <summary>
        /// 表示可能座標の確認で利用するマージン値を取得・設定します。
        /// 値の設定は初回のみ可能です。
        /// </summary>
        public double Margin
        {
            get
            {
                if (!_margin.HasValue)
                {
                    throw new InvalidOperationException("MutiMonitorUtil.Margin は初期化されていません。");
                }
                return _margin.Value;
            }
            set
            {
                if (_margin.HasValue)
                {
                    throw new InvalidOperationException("MutiMonitorUtil.Margin は既に初期化されています。");
                }
                _margin = value;
            }
        }

        /// <summary>
        /// スクリーン情報をリフレッシュします。
        /// </summary>
        /// <returns>情報の更新があった場合は true を返します。</returns>
        public bool Refresh()
        {
            var temp = FORM::Screen.AllScreens;

            lock (_lockScreens)
            {
                if (_screens == null)
                {
                    _screens = temp;
                    return false;
                }

                if (_screens.Length != temp.Length)
                {
                    _screens = temp;
                    return true;
                }

                for (var i = 0; i < _screens.Length; i++)
                {
                    if (_screens[i].DeviceName != temp[i].DeviceName)
                    {
                        _screens = temp;
                        return true;
                    }

                    if (_screens[i].WorkingArea.Left != temp[i].WorkingArea.Left ||
                        _screens[i].WorkingArea.Top != temp[i].WorkingArea.Top ||
                        _screens[i].WorkingArea.Right != temp[i].WorkingArea.Right ||
                        _screens[i].WorkingArea.Bottom != temp[i].WorkingArea.Bottom)
                    {
                        _screens = temp;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// プライマリモニターとなっているモニターの名前(デバイス名)を返します。
        /// </summary>
        /// <returns>モニターの名前(デバイス名)</returns>
        public string GetPrimaryMonitorName()
        {
            return GetPrimaryMinitor().DeviceName;
        }

        /// <summary>
        /// モニター名(デバイス名)の一致するモニターの表示エリアを返します。
        /// </summary>
        /// <param name="monitorName">モニター名(デバイス名)</param>
        /// <returns>表示エリアを示す Rect 構造体</returns>
        /// <exception cref="System.ArgumentException">モニター名(デバイス名)が存在しない場合に発生します。</exception>
        public Rect GetWorkingArea(string monitorName)
        {
            FORM::Screen screen;
            lock (_lockScreens)
            {
                screen = _screens.FirstOrDefault(p => p.DeviceName == monitorName);
            }
            if (screen == null)
            {
                throw new ArgumentException("モニター名(デバイス名)が存在しません。");
            }
            return Rectangle2rect(screen.WorkingArea);
        }

        /// <summary>
        /// 与えられた Rect 構造体の座標がマルチモニターの表示範囲内に収まっているかの真偽値を返します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>真偽値(真であれば表示範囲内)</returns>
        public bool IsInRange(Rect target)
        {
            return GetInRangeMonitorName(target) != null;
        }

        /// <summary>
        /// 与えられた Rect 構造体の座標が表示範囲内に収まっているモニター名を返します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>モニター名(座標がすべてのモニターの表示範囲から外れている場合は null)</returns>
        public string GetInRangeMonitorName(Rect target)
        {
            return GetInRangeMonitorName(target, Margin);
        }

        /// <summary>
        /// 与えられた Rect 構造体の座標が表示範囲内に収まっているモニター名を返します。
        /// このメソッドはマージン値を考慮せずに判定します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>モニター名(座標がすべてのモニターの表示範囲から外れている場合は null)</returns>
        public string GetInRangeMonitorNameExcludingMargin(Rect target)
        {
            return GetInRangeMonitorName(target, 0);
        }

        private string GetInRangeMonitorName(Rect target, double margin)
        {
            lock (_lockScreens)
            {
                foreach (var n in _screens)
                {
                    if (target.Right - margin >= n.WorkingArea.Left && target.Left + margin <= n.WorkingArea.Right &&
                        target.Bottom - margin >= n.WorkingArea.Top && target.Top + margin <= n.WorkingArea.Bottom)
                    {
                        return n.DeviceName;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 与えられた Rect 構造体の座標の中心が位置するモニター名を返します。
        /// </summary>
        /// <param name="target">判別する表示座標を格納したRect 構造体</param>
        /// <returns>モニター名(座標の中心がすべてのモニターの表示範囲から外れている場合は null)</returns>
        public string GetMoniterNameCenterPosition(Rect target)
        {
            var x = target.Left + ((target.Right - target.Left) / 2);
            var y = target.Top + ((target.Bottom - target.Top) / 2);
            var rect = new Rect(x, y, 0, 0);
            return GetInRangeMonitorName(rect, 0);
        }

        /// <summary>
        /// モニター名とモニター上の相対座標から表示座標を得ます。
        /// モニター名が存在しない場合は、プライマリモニター上の座標を返します。
        /// 得られた座標が表示可能域を外れていた場合は、画面中央の座標を返します。
        /// </summary>
        /// <param name="target">モニター上の相対座標と大きさを保持する Rect 構造体</param>
        /// <param name="monitorName">表示するモニター名</param>
        /// <returns>ウィンドウを表示する座標</returns>
        public Point GetDisplayPosition(Rect target, string monitorName)
        {
            var point = GetAbsolutePoint(target.TopLeft, monitorName);
            var rect = new Rect(point, target.Size);
            if (!IsInRange(rect))
            {
                var screen = GetScreenByName(monitorName);
                if (screen == null)
                {
                    screen = GetPrimaryMinitor();
                }

                point = new Point(screen.WorkingArea.Width / 2 - target.Width / 2 + screen.WorkingArea.Left,
                                  screen.WorkingArea.Height / 2 - target.Height / 2 + screen.WorkingArea.Top);
            }

            return point;
        }

        /// <summary>
        /// 与えられた Rect 構造体の座標から相対座標情報を得ます。
        /// </summary>
        /// <param name="target">Window の座標を格納した Rect 構造体</param>
        /// <returns>相対座標情報を格納した RelativePointInfo クラスのインスタンス</returns>
        public RelativePointInfo GetRelativePointInfo(Rect target)
        {
            var monitorName = GetMoniterNameCenterPosition(target) ?? GetInRangeMonitorName(target);

            var screen = GetScreenByName(monitorName);
            var point = new Point(target.Left - screen.WorkingArea.Left, target.Top - screen.WorkingArea.Top);

            return new RelativePointInfo(point, monitorName);
        }

        /// <summary>
        /// 与えられた Rect 構造体の座標とモニター名から相対座標情報を得ます。
        /// </summary>
        /// <param name="target">Window の座標を格納した Rect 構造体</param>
        /// <param name="monitorName">相対座標を得るモニターの名前</param>
        /// <returns>相対座標を格納した Point 構造体</returns>
        public Point GetRelativePoint(Rect target, string monitorName)
        {
            var screen = GetScreenByName(monitorName);
            var point = new Point(target.Left - screen.WorkingArea.Left, target.Top - screen.WorkingArea.Top);

            return point;
        }

        /// <summary>
        /// モニター名とモニター上の相対座標から表示座標を得ます。
        /// モニター名が存在しない場合は、プライマリモニター上の座標を得ます。
        /// </summary>
        /// <param name="relativePoint">相対座標</param>
        /// <param name="monitorName">モニター名</param>
        /// <returns>表示座標</returns>
        private Point GetAbsolutePoint(Point relativePoint, string monitorName)
        {
            double x = 0, y = 0;

            var screen = GetScreenByName(monitorName);
            if (screen != null)
            {
                x = screen.WorkingArea.Left;
                y = screen.WorkingArea.Top;
            }

            return new Point(x + relativePoint.X, y + relativePoint.Y);
        }

        private FORM::Screen GetScreenByName(string monitorname)
        {
            FORM::Screen screen;
            lock (_lockScreens)
            {
                screen = _screens.FirstOrDefault(p => p.DeviceName == monitorname);
            }
            return screen;
        }

        private FORM::Screen GetPrimaryMinitor()
        {
            FORM::Screen screen;
            lock (_lockScreens)
            {
                screen = _screens.First(p => p.Primary == true);
            }
            return screen;
        }

        private Rect Rectangle2rect(System.Drawing.Rectangle rectangle)
        {
            return new Rect(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }
    }
}
