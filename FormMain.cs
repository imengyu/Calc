using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Calc
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Height = 261;
            CheckForIllegalCrossThreadCalls = false;
        }

        private string lastEnter = "";
        private string lastEnterGy = "";

        private void buttonAc_Click(object sender, EventArgs e)
        {
            textBoxEnter.Text = "";
            textBoxRs.Text = "0";
            textBoxEnter.Focus();
        }
        private void buttonCalc_Click(object sender, EventArgs e)
        {
            if (lastEnter != textBoxEnter.Text || textBoxEnter.Text.Contains("rand(") || textBoxEnter.Text.Contains("rnd("))
            {
                if (lastEnter != textBoxEnter.Text)
                {
                    lastEnter = textBoxEnter.Text;
                    textBoxEnter.Items.Insert(0, lastEnter);
                }
                textBoxRs.Text = AutoCalc.Calc(textBoxEnter.Text.Replace(" ", ""));
            }
        }

        private void radio_d_CheckedChanged(object sender, EventArgs e)
        {
            AutoCalc.UseDegree = true;
        }
        private void radio_g_CheckedChanged(object sender, EventArgs e)
        {
            AutoCalc.UseDegree = false;
        }

        private void textBoxEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData== Keys.Enter)
            {
                buttonCalc_Click(sender, e);
            }
        }

        private void insert_to_enter(string s)
        {
            int old = textBoxEnter.SelectionStart;
            textBoxEnter.Text = textBoxEnter.Text.Insert(textBoxEnter.SelectionStart,s);
            textBoxEnter.Focus();
            textBoxEnter.SelectionLength = 0;
            if (old == 0) textBoxEnter.SelectionStart = textBoxEnter.Text.Length;
            else textBoxEnter.SelectionStart = old+1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            insert_to_enter("√");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            insert_to_enter("log(");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            insert_to_enter("ln(");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            insert_to_enter("abs(");
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            insert_to_enter("sin(");
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            insert_to_enter("cos(");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            insert_to_enter("tan(");
        }
        private void button8_Click(object sender, EventArgs e)
        {
            insert_to_enter("lg(");
        }
        private void button9_Click(object sender, EventArgs e)
        {
            insert_to_enter("asin(");
        }
        private void button11_Click(object sender, EventArgs e)
        {
            insert_to_enter("acos(");
        }
        private void button10_Click(object sender, EventArgs e)
        {
            insert_to_enter("atan(");
        }
        private void button12_Click(object sender, EventArgs e)
        {
            insert_to_enter("fix(");
        }

        private Bitmap fungraph;

        private void FormMain_Load(object sender, EventArgs e)
        {
            fungraph = new Bitmap(434, 296, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            textBoxEnter.Focus();
            textBoxEnter.SelectionStart = 0;
            AutoCalc.CalcFx += AutoCalc_CalcFx;
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            fungraph.Dispose();
            fungraph = null;
        }

        private void 数字键盘开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_numpad.Visible = true;
            if (panel_fungraph.Visible) panel_fungraph.Hide();
            Top -= (442 - Height)/2;
            Height = 442;
            数字键盘开启ToolStripMenuItem.Checked = true;
            数字键盘关闭ToolStripMenuItem.Checked = false;
        }
        private void 数字键盘关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_numpad.Visible = false;
            Height = 261;
            Top += (442 - 261)/2;
            数字键盘开启ToolStripMenuItem.Checked = false;
            数字键盘关闭ToolStripMenuItem.Checked = true;
        }

        //按钮键盘输入
        private void button22_Click(object sender, EventArgs e)
        {
            insert_to_enter("5");
        }
        private void button17_Click(object sender, EventArgs e)
        {
            int old = textBoxEnter.SelectionStart;
            if (old > 0)
            {
                if (textBoxEnter.SelectionLength != 0)
                {
                    textBoxEnter.Text = textBoxEnter.Text.Remove(textBoxEnter.SelectionStart, textBoxEnter.SelectionLength);
                    textBoxEnter.Focus();
                    textBoxEnter.SelectionStart = old;
                    textBoxEnter.SelectionLength = 0;
                }
                else
                {
                    textBoxEnter.Text = textBoxEnter.Text.Remove(textBoxEnter.SelectionStart - 1, 1);
                    textBoxEnter.Focus();
                    textBoxEnter.SelectionStart = old - 1;
                }
            }
        }
        private void button35_Click(object sender, EventArgs e)
        {
            insert_to_enter("(");
        }
        private void button34_Click(object sender, EventArgs e)
        {
            insert_to_enter(")");
        }
        private void button16_Click(object sender, EventArgs e)
        {
            insert_to_enter("+");
        }
        private void button20_Click(object sender, EventArgs e)
        {
            insert_to_enter("-");
        }
        private void button25_Click(object sender, EventArgs e)
        {
            insert_to_enter("×");
        }
        private void button30_Click(object sender, EventArgs e)
        {
            insert_to_enter("÷");
        }
        private void button32_Click(object sender, EventArgs e)
        {
            insert_to_enter(".");
        }
        private void button33_Click(object sender, EventArgs e)
        {
            insert_to_enter("0");
        }
        private void button28_Click(object sender, EventArgs e)
        {
            insert_to_enter("1");
        }
        private void button27_Click(object sender, EventArgs e)
        {
            insert_to_enter("2");
        }
        private void button26_Click(object sender, EventArgs e)
        {
            insert_to_enter("3");
        }
        private void button23_Click(object sender, EventArgs e)
        {
            insert_to_enter("4");
        }
        private void button21_Click(object sender, EventArgs e)
        {
            insert_to_enter("6");
        }
        private void button13_Click(object sender, EventArgs e)
        {
            insert_to_enter("7");
        }
        private void button14_Click(object sender, EventArgs e)
        {
            insert_to_enter("8");
        }
        private void button15_Click(object sender, EventArgs e)
        {
            insert_to_enter("9");
        }
        private void button37_Click(object sender, EventArgs e)
        {
            insert_to_enter("f(x)=");
        }

        private const int fungraph_title_height = 26;
        private bool fungraph_needrsdaw = true;
        private bool fungraph_dawing=false;
        private string fungraph_xy_to_str(double d)
        {
            if (d % Math.PI == 0)
            {
                int dx = (int)(d / Math.PI);
                if (dx == 1)
                    return "π";
                else if (dx == -1)
                    return "-π";
                else return (dx) + "π";
            }
            else
            {
                string sx = d.ToString("0.0000");
                if (sx.EndsWith(".0000"))
                    sx = sx.Remove(sx.Length - 5, 5);
                if (sx.EndsWith("0"))
                    sx = sx.Remove(sx.Length - 1, 1);
                if (sx.EndsWith("00"))
                    sx = sx.Remove(sx.Length - 2, 2);
                if (sx.EndsWith("000"))
                    sx = sx.Remove(sx.Length - 3, 3);
                return sx;
            }
        }
        private string fungraph_str_to_xy(string s)
        {
            string rs = s;
            if (s.Contains("pi"))
            {
                rs = rs.Replace("pi", "π");
            }
            if (s.Contains("π"))
            {
                int ix = s.IndexOf('π');
                if (ix > 0 && !AutoCalc.CalcIsNoNumChar(s[ix - 1]))
                    rs=s.Insert(ix, "*");
                rs = rs.Replace("π", "Math.PI");
            }
            if (s.Contains("e"))
            {
                for (int i = 0; i < rs.Length; i++)
                {
                    if (rs[i] == 'e')
                    {
                        bool replace = true;
                        if (i > 0) if (AutoCalc.CalcIsNoNumChar(rs[i - 1])) replace = false;
                        if (i + 1 < rs.Length) if (AutoCalc.CalcIsNoNumChar(rs[i + 1])) replace = false;
                        if (i - 1 > 0) if (!AutoCalc.CalcIsNoNumChar(rs[i - 1])) s.Insert(i - 1, "*");
                        if (replace)
                        {
                            rs = rs.Remove(i, 1);
                            rs = rs.Insert(1, "Math.E");
                        }
                    }
                }
            }
            return rs;
        }
        
        private bool draw_fungraph(string fuuuus)
        {
            //绘制函数
            if (fuuuus.StartsWith("f(x)=")) fuuuus = fuuuus.Remove(0, 5);
            if (fungraph_needrsdaw || lastEnterGy != fuuuus)
            {
                string fun = fuuuus;
                if (lastEnterGy != fuuuus || lastEnterGy=="")
                {
                    lastEnterGy = fuuuus;
                    fungraph_dawing = true;

                    if (fun.Contains("sin(") && !fun.Contains("asin(")) fun = fun.Replace("sin(", "Math.Sin(");
                    if (fun.Contains("cos(") && !fun.Contains("acos(")) fun = fun.Replace("cos(", "Math.Cos(");
                    if (fun.Contains("tan(") && !fun.Contains("atan(")) fun = fun.Replace("tan(", "Math.Tan(");
                    if (fun.Contains("asin(")) fun = fun.Replace("asin(", "Math.Asin(");
                    if (fun.Contains("acos(")) fun = fun.Replace("acos(", "Math.Acos(");
                    if (fun.Contains("atan(")) fun = fun.Replace("atan(", "Math.Atan(");
                    if (fun.Contains("pow(")) fun = fun.Replace("pow(", "Math.Pow(");
                    if (fun.Contains("lg(")) fun = fun.Replace("lg(", "Math.Log10(");
                    if (fun.Contains("log(")) fun = fun.Replace("log(", "Math.Log(");
                    if (fun.Contains("ln(")) fun = fun.Replace("ln(", "Math.Log(");
                    if (fun.Contains("sqrt(")) fun = fun.Replace("sqrt(", "Math.Sqrt(");
                    if (fun.Contains("ln(")) fun = fun.Replace("ln(", "Math.Log(");
                    if (fun.Contains("π")) fun = fun.Replace("π", "Math.PI");
                    if (fun.Contains("pi")) fun = fun.Replace("pi", "Math.PI");
                    if (fun.Contains("e")) fun = fun.Replace("e", "Math.E");

                    lock (fungraph)
                    {
                        Graphics gc = Graphics.FromImage(fungraph);
                        gc.Clear(Color.White);
                        try
                        {
                            if (fun == "")
                                throw new Exception("请输入需要绘制的函数！");
                            else if (!fun.Contains("x"))
                                throw new Exception("未找到自变量x");

                            double x0 = new Expression(fungraph_str_to_xy(tbxX0.Text)).Compute(0);
                            double x1 = new Expression(fungraph_str_to_xy(tbxX1.Text)).Compute(0);
                            if (x1 <= x0)
                                throw new Exception("区间设置有误。请保证区间 结束>开始 。");
                            Size size = panel_fungraph.Size;
                            int i0 = 0;
                            int i1 = size.Width - 1;
                            int j0 = fungraph_title_height;// 屏幕保留区域的高度 
                            int j1 = size.Height - 1;
                            Pen pen = new Pen(Color.Black, 1);
                            //gc.DrawLine(pen, i0, j0, i1, j0); // 画图区和保留区的分界线 
                            double rx = (x1 - x0) / (i1 - i0);
                            double y0, y1;
                            Expression fx = new Expression(fun);
                            fungraph_GetFunctionValueRange(fx, x0, rx, i0, i1, out y0, out y1);

                            double ry = (y1 - y0) / (j1 - j0);
                            if (check_fungr_data.Checked)
                            {
                                fungraph_Out(gc, 1, "f(x): " + fun);
                                fungraph_Out(gc, 3, "x:[{0}, {1}] ({2})", fungraph_xy_to_str(x0), fungraph_xy_to_str(x1), fungraph_xy_to_str(x1 - x0));
                                fungraph_Out(gc, 4, "y:[{0}, {1}] ({2})", fungraph_xy_to_str(y0), fungraph_xy_to_str(y1), fungraph_xy_to_str(y1 - y0));
                                fungraph_Out(gc, 6, "rx:{0}", (1 / rx).ToString("0.0000"));  // 函数自变量每单位值用多少个象素表示 
                                fungraph_Out(gc, 7, "ry:{0}", (1 / ry).ToString("0.0000"));  // 函数的值每单位值用多少个象素表示 
                                fungraph_Out(gc, 8, "r :{0}", (rx / ry).ToString("0.0000")); // 该值如果小于1表示图形纵向被压扁，反之则被拉伸 
                            }

                            pen.Color = Color.Green;
                            int j = j1 + (int)(y0 / ry);
                            if (j >= j0 && j <= j1)
                                if (check_fg_drawxy.Checked)
                                {
                                    gc.DrawLine(pen, i0, j, i1, j); // x坐标轴
                                    gc.DrawString(fungraph_xy_to_str(x0), new Font("Courier New", 10), Brushes.Black, 0, j - 15);
                                    gc.DrawString("0", new Font("Courier New", 10), Brushes.Blue, size.Width / 2, j - 15);
                                    gc.DrawString(fungraph_xy_to_str(x1), new Font("Courier New", 10), Brushes.Black, new Rectangle(size.Width - 30, j - 15, 30, 15), new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far });
                                }
                                else
                                {
                                    pen.Color = Color.Silver;
                                    gc.DrawLine(pen, i0, j, i1, j); // x坐标轴
                                }
                            int i = i0 - (int)(x0 / rx);
                            if (i >= i0 && i <= i1)
                                if (check_fg_drawxy.Checked)
                                {
                                    gc.DrawLine(pen, i, j0, i, j1); // y坐标轴 
                                    gc.DrawString(fungraph_xy_to_str(y1), new Font("Courier New", 10), Brushes.Black, i + 1, fungraph_title_height);
                                    gc.DrawString(fungraph_xy_to_str(y0), new Font("Courier New", 10), Brushes.Black, i + 1, j1 - 15);
                                }
                                else
                                {
                                    pen.Color = Color.Silver;
                                    gc.DrawLine(pen, i, j0, i, j1); // y坐标轴 
                                }
                            pen.Color = Color.Red;
                            for (i = i0; i <= i1; i++)
                            {
                                double x = x0 + (i - i0) * rx;
                                double y = fx.Compute(x);
                                if (double.IsInfinity(y) || double.IsNaN(y)) continue;
                                j = j1 - (int)((y - y0) / ry);
                                if (j > j1 || j < j0) continue;
                                gc.DrawLine(pen, i, j, i + 1, j); // 画函数的图形 
                            }
                        }
                        catch (Exception ex)
                        {
                            StringFormat f = new StringFormat();
                            f.LineAlignment = StringAlignment.Center;
                            f.Alignment = StringAlignment.Center;
                            gc.DrawString(ex.Message, new Font("Courier New", 10), Brushes.Blue, new Rectangle(5, fungraph_title_height, panel_fungraph.Width, panel_fungraph.Height), f);

                            fungraph_needrsdaw = false;
                            button31.Text = "绘制函数图像";
                            fungraph_dawing = false;
                            panel_fungraph.Invalidate();
                            return false;
                        }
                    }
                }
                fungraph_needrsdaw = false;
            }
            button31.Text = "绘制函数图像";
            fungraph_dawing = false;
            panel_fungraph.Invalidate();
            return true;
        }
        private void panel_fungraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics gc = e.Graphics;
            gc.DrawImage(fungraph, 0, 0);
        }      
        private  void fungraph_GetFunctionValueRange(Expression fx, double x0, double rx, int i0, int i1, out double y0, out double y1)
        {
            // 函数值的取值范围 
            y0 = double.MaxValue;
            y1 = double.MinValue;
            for (int i = i0; i <= i1; i++)
            {
                double x = x0 + (i - i0) * rx;
                double y = fx.Compute(x);
                if (double.IsInfinity(y) || double.IsNaN(y)) continue;
                if (y0 > y) y0 = y;
                if (y1 < y) y1 = y;
            }
        } 
        private void fungraph_Out(Graphics gc, int line, string fmt, params object[] args)
        {
            // 在指定的位置写字符串 
            gc.DrawString(string.Format(fmt, args), new Font("Courier New", 10), Brushes.Blue, new PointF(5, fungraph_title_height + 10 * line));
        }
        private void button36_Click(object sender, EventArgs e)
        {
            Height = 261;
            Top += (558 - 261) / 2;
            panel_fungraph.Visible = false;
        }
        private void button31_Click(object sender, EventArgs e)
        {
            Top -= (558 - Height) / 2;
            Height = 558;
            panel_fungraph.Visible = true;

            button31.Text = "正在绘制...";
            if (!fungraph_dawing)
            {
                Thread thread1 = new Thread(delegate () { draw_fungraph(textBoxEnter.Text.Replace(" ", "")); });
                fungraph_dawing = true;
                thread1.Start();
            }
        }
        private void check_fg_drawxy_CheckedChanged(object sender, EventArgs e)
        {
            if (!fungraph_needrsdaw) fungraph_needrsdaw = true;
        }
        private void check_fungr_data_CheckedChanged(object sender, EventArgs e)
        {
            if (!fungraph_needrsdaw) fungraph_needrsdaw = true;
        }
        private void tbxX0_TextChanged(object sender, EventArgs e)
        {
            if (!fungraph_needrsdaw) fungraph_needrsdaw = true;
        }
        private void tbxX1_TextChanged(object sender, EventArgs e)
        {
            if (!fungraph_needrsdaw) fungraph_needrsdaw = true;
        }

        private string AutoCalc_CalcFx(object sender, string e)
        {
            Top -= (558 - Height) / 2;
            Height = 558;
            panel_fungraph.Visible = true;
            if (!fungraph_dawing)
            {
                Thread thread1 = new Thread(delegate () {
                    draw_fungraph(e);
                });
                fungraph_dawing = true;
                thread1.Start();
            }
            return "函数图像绘制";
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
     
        }
        private void 帮助关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormAbout().ShowDialog();
        }
        private void 所有支持的函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormFuns().ShowDialog();
        }
        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEnter.Text = "rnd(1,10)";
        }
        private void 函数绘制窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Top -= (558 - Height) / 2;
            Height = 558;
            panel_fungraph.Visible = true;
            if (lastEnterGy == "")
            {
                lock (fungraph)
                {
                    Graphics gc = Graphics.FromImage(fungraph);
                    gc.Clear(Color.White);
                    StringFormat f = new StringFormat();
                    f.LineAlignment = StringAlignment.Center;
                    f.Alignment = StringAlignment.Center;
                    gc.DrawString("在算式一栏输入您想绘制的函数，点击“绘制函数图像”按钮开始绘制", new Font("Courier New", 10), Brushes.Blue, new Rectangle(5, 21 + 0, panel_fungraph.Width, panel_fungraph.Height), f);
                }
                button31.Text = "绘制函数图像";
                fungraph_dawing = false;
                panel_fungraph.Invalidate();
            }
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void 清空历史记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEnter.Items.Clear();
            textBoxEnter.Items.Add("清空历史记录");
        }
        private void textBoxEnter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBoxEnter.SelectedIndex == textBoxEnter.Items.Count - 1)
                清空历史记录ToolStripMenuItem_Click(sender, e);
            else textBoxRs.Text = "";
        }

        private void buttonTo10_Click(object sender, EventArgs e)
        {
            xtox(8, "八进制");
        }
        private void buttonToB_Click(object sender, EventArgs e)
        {
            xtox(2, "二进制");
        }
        private void buttonToH_Click(object sender, EventArgs e)
        {
            xtox(16, "十六进制");
        }
        private void xtox(int x, string sx)
        {
            string sr = "";
            string s = textBoxEnter.Text;

            double d = 0;
            if (double.TryParse(s, out d))
            {
                if ((int)d == d)
                {
                    sr = s + "→" + Convert.ToString((int)d, x) + " ("+ sx + ")";
                    textBoxRs.Text = sr;
                }
                else sr = " 进制转换必须是整数";
                textBoxRs.Text = sr;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textBoxEnter.Text = "3 * (2 + (3 - 6))"; 
        }













        //insert_to_enter("");
    }
}
