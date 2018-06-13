using System;

namespace Calc
{
    //
    //此类仅为计算用户输入的算式
    //程序需要计算-请直接用代码
    //效率一般

    /// <summary>
    /// 自动算式计算
    /// </summary>
    public class AutoCalc
    {
        public delegate string CalcFxEventHandler(object sender, string e);

        /// <summary>
        /// fx回调
        /// </summary>
        public static event CalcFxEventHandler CalcFx;

        //
        //直接传入需要计算的算式
        //返回就是结果
        //
        /// <summary>
        /// 计算入口
        /// </summary>
        /// <param name="equation">需要计算的算式</param>
        /// <returns>返回结果</returns>
        public static string Calc(string equation)
        {
            if (string.IsNullOrEmpty(equation))//空检测
                return "请输入算式";
            if (equation.StartsWith("="))//处理掉=
                equation = equation.Remove(0, 1);
            else if (equation.StartsWith("f(x)="))//f(x)=是函数绘制的命令，在formmain中绘制
            {
                equation = equation.Remove(0, 5);
                return CalcFx(null, equation);
            }
            try
            {
                //计算
                return CalcCore(equation);
            }
            catch (Exception e)
            {
                //异常信息抓取
                return e.Message;
            }
        }
        /// <summary>
        /// 是否使用角度制
        /// </summary>
        public static bool UseDegree
        {
            get;set;
        }

        //计算主函数
        private static string CalcCore(string s)
        {
            if (s == "") return s;
            double b = 0;
            if (s.Contains("(")) s = CalcParenthesis(s);//先计算括号
            if (s.Contains(",")) //识别为函数参数
            {
                string[] ss = s.Split(',');
                s = "";
                for (int i = 0; i < ss.Length; i++)//参数逐一分解
                {
                    ss[i] = CalcCore(ss[i]);
                    s += ss[i];
                    if (i != ss.Length - 1)
                        s += ",";
                }
            }
            else
            {
                s = CalcConstants(s);//再替换常量
                if (s.Contains("+") || (s.Contains("-") && !s.StartsWith("-") && CalcCheckDiv(s))) s = CalcOp_AddSub(s);//计算+-
                else if (s.Contains("%") || s.Contains("MOD")) s = CalcOp_Mod(s);//计算% MOD
                else if (s.Contains("*") || s.Contains("/") || s.Contains("×") || s.Contains("÷")) s = CalcOp_MultDiv(s);//计算* /
                else if (s.Contains("^")) s = CalcOp_Pow(s);//计算乘幂
                else if (s.Contains("√")) s = CalcOp_Sqrt(s);//计算平方根
                else if (s.Contains("!")) s = CalcOp_Factorial(s);//计算阶乘
                else if (double.TryParse(s, out b))//没有运算符
                    s = b.ToString();//如果可以转为数字，输出数字
                else throw new Exception("错误的输入：" + s);//无法处理，抛出异常
            }
            return s;
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="d">角度</param>
        /// <returns>弧度</returns>
        public static double CalcD2G(double d)
        {
            return Math.PI * d / 180d;
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="g">弧度</param>
        /// <returns>角度</returns>
        public static double CalcG2D(double g)
        {
            return 180 * g / Math.PI;
        }
        /// <summary>
        /// char是否是op运算符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CalcIsOp(char s)
        {
            return (s == ' '
                    || s == '+' || s == '-' || s == '*' || s == '/' || s == '\\' || s == '^'
                    || s == '%' || s == '×' || s == '÷' || s == '！' || s == 'A'
                    || s == 'C' || s == '√' || s == '‰');
        }
        /// <summary>
        /// char是否是非数字（运算符）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CalcIsNoNumChar(char s)
        {
            return (s == ' '
                    || s == '+' || s == '-' || s == '*' || s == '/' || s == '\\' || s == '^'
                    || s == '%' || s == '&' || s == '×' || s == '÷' || s == '！' || s == 'A'
                    || s == 'C' || s == '√' || s == '‰' || s == '‱' || s == '∑' || s == '∫');
        }

        private static Random random = null;
        private static bool CalcCheckDiv(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '-')
                {
                    if (i > 0)
                    {
                        if (CalcIsOp(s[i - 1]))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private static double CalcFactorial(double d)
        {
            //计算阶乘
            double sum = 1;
            for (int i = 1; i <= d; i++)
            {
                sum = sum * i;
            }
            return sum;
        }
        private static string CalcConstants(string s)
        {
            //把字符串中含有的常量转换成数字 支持 pi 、e、π
            string rs = s;
            if (s.Contains("pi"))
            {
                rs = rs.Replace("pi", "π");
            }
            if (s.Contains("π"))
            {
                int ix = s.IndexOf('π');
                if (ix > 0 && !CalcIsNoNumChar(s[ix - 1]))
                    rs=s.Insert(ix, "*");
                rs = rs.Replace("π", Math.PI.ToString());
            }
            if (s.Contains("e"))
            {
                for (int i = 0; i < rs.Length; i++)
                {
                    if (rs[i] == 'e')
                    {
                        bool replace = true;
                        if (i > 0) if (CalcIsNoNumChar(rs[i - 1])) replace = false;
                        if (i + 1 < rs.Length) if (CalcIsNoNumChar(rs[i + 1])) replace = false;
                        if (i - 1 > 0) if (!CalcIsNoNumChar(rs[i - 1])) s.Insert(i - 1, "*");
                        if (replace)
                        {
                            rs = rs.Remove(i, 1);
                            rs = rs.Insert(1, Math.E.ToString());
                        }
                    }
                }
            }
            return rs;
        }
        private static string CalcFindFrontName(string s)
        {
            //获取函数名 
            //  比如 sin(3)
            //  获取的就是 sin
            string rs = s;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (CalcIsNoNumChar(s[i]))
                {
                    rs = s.Substring(i + 1);
                    break;
                }
            }
            return rs;
        }
        private static string CalcFunction(string s, string sf)
        {
            //函数计算
            //  s 是函数的参数 sf 是函数名
            // 比如 sin(3)
            // s="3" sf="sin"
            string rs = "0";
            double n = 0;
            if (sf == "sin" || sf == "cos" || sf == "tan"
                || sf == "asin" || sf == "acos" || sf == "atan"
                 || sf == "sinh" || sf == "cosh" || sf == "tanh")//过滤无参数的函数和其他函数
                try
                {
                    //根据设置转换文字（角度/弧度）为弧度
                    n = UseDegree ? CalcD2G(double.Parse(s)) : double.Parse(s);
                }
                catch
                {
                    throw new Exception("未知符号：" + s);
                }
            switch (sf)
            {
                //一下是一些常用数学函数
                case "sin":
                    rs = Math.Sin(n).ToString();
                    break;
                case "cos":
                    rs = Math.Cos(n).ToString();
                    break;
                case "tan":
                    rs = Math.Tan(n).ToString();
                    break;
                case "sinh":
                    rs = Math.Sinh(n).ToString();
                    break;
                case "cosh":
                    rs = Math.Cosh(n).ToString();
                    break;
                case "tanh":
                    rs = Math.Tanh(n).ToString();
                    break;
                case "asin":
                    rs = Math.Asin(n).ToString();
                    break;
                case "acos":
                    rs = Math.Acos(n).ToString();
                    break;
                case "atan":
                    rs = Math.Atan(double.Parse(s)).ToString();
                    break;
                case "lg":
                    rs = Math.Log10(double.Parse(s)).ToString();
                    break;
                case "ln":
                    rs = Math.Log(double.Parse(s)).ToString();
                    break;
                case "exp":
                    rs = Math.Exp(double.Parse(s)).ToString();
                    break;
                case "abs":
                    rs = Math.Abs(double.Parse(s)).ToString();
                    break;
                case "√":
                    rs = Math.Sqrt(double.Parse(s)).ToString();
                    break;
                case "fix":
                    rs = Math.Floor(double.Parse(s)).ToString();
                    break;
                case "log":
                    if(s.Contains(","))
                    {
                        string[] ss = s.Split(',');
                        if(ss.Length!=2) throw new Exception("函数：log 需要两个参数");
                        rs = Math.Log(double.Parse(ss[0]), double.Parse(ss[1])).ToString();
                    }
                    else throw new Exception("函数：log 需要两个参数");
                    break;
                case "rand":
                    if (random == null) random = new Random();
                    rs = random.NextDouble().ToString();
                    break;
                case "rnd":
                    if (s.Contains(","))
                    {
                        if (random == null) random = new Random();
                        string[] ss = s.Split(',');
                        if (ss.Length != 2) throw new Exception("函数：rnd 需要两个参数");
                        rs = random.Next(int.Parse(ss[0]), int.Parse(ss[1])).ToString();
                    }
                    else throw new Exception("函数：log 需要两个参数");
                    break;
                default:
                    {
                        double nr;
                        if (double.TryParse(sf, out nr))
                            rs = Math.Floor(nr * double.Parse(s)).ToString();
                        else throw new Exception("未知函数：" + sf);
                        break;
                    }
            }
            return rs;
        }
        private static string CalcParenthesis(string s)
        {
            //递归处理算式中的括号
            //先从外到里
            bool qfinded = false;
            int i, qc = 0, qstart = 0;
            for (i = 0; i < s.Length; i++)
            {
                //括号计数
                //
                if (s[i] == '(')
                {
                    qc++;
                    if (!qfinded) qfinded = true;
                    if (qc == 1) qstart = i;
                }
                else if (s[i] == ')') qc--;
                if (qc == 0 && qfinded)
                {
                    //括号结尾
                    qfinded = false;
                    string sqs = s.Substring(qstart + 1, i - qstart - 1);
                    //找括号前面是否有函数名
                    string sfs = CalcFindFrontName(s.Substring(0, qstart));
                    //递归将括号中的算式算掉
                    string srs = CalcCore(sqs);
                    //如果有函数名则计算函数
                    if (sfs != "")
                    {
                        srs = CalcFunction(srs, sfs);
                        //结果替换到原算式中
                        string sfss = sfs + "(";
                        int ixx = s.IndexOf(sfss);
                        s = s.Remove(ixx, sfss.Length);
                    }
                    else
                    {
                        //删除第一个(
                        int ixx = s.IndexOf('(');
                        s = s.Remove(ixx, 1);
                    }
                    s = s.Replace(sqs + ")", srs);
                }
            }
            return s;
        }

        private static string CalcOpSingal(string sl, string sr, string op)
        {
            //计算单个运算符
            //优先级 由上到下
            //是N说明是第一次运算，直接返回
            if (sl == "N" && (op != "!" && op != "√")) return sr;
            string rs = "0";
            double nl = 0, nr = 0;
            double.TryParse(sl, out nl);
            double.TryParse(sr, out nr);
            switch (op)
            {
                case "+":
                    rs = (nl + nr).ToString();
                    break;
                case "-":
                    rs = (nl - nr).ToString();
                    break;
                case "*":
                case "×":
                    rs = (nl * nr).ToString();
                    break;
                case "/":
                case "÷":
                    if (nr == 0) throw new Exception("除数为0");
                    rs = (nl / nr).ToString();
                    break;
                case "^":
                    rs = (Math.Pow(nl, nr)).ToString();
                    break;
                case "MOD":
                case "%":
                    rs = (nl % nr).ToString();
                    break;
                case "√":
                    rs = (Math.Sqrt(nr).ToString());
                    break;
                case "!":
                    rs = (CalcFactorial(nr)).ToString();
                    break;
            }
            return rs;
        }
        private static bool CalcOp_HasMoreAdvancedOp(string s, string cu)
        {
            //查看当前算式中是否有更优先的运算符
            //cu 当前运算符优先标识
            // s 算式
            switch (cu)
            {
                case "+-":
                    return (s.Contains("*") || s.Contains("/") || s.Contains("×") || s.Contains("÷") || s.Contains("^")
                        || s.Contains("%") || s.Contains("√") || s.Contains("!") || s.Contains("MOD"));
                case "%":
                    return (s.Contains("*") || s.Contains("/") || s.Contains("×") || s.Contains("÷") || s.Contains("^")
                        || s.Contains("√") || s.Contains("!"));
                case "*/":
                    return (s.Contains("√") || s.Contains("!") || s.Contains("^"));
                case "^":
                    return (s.Contains("√") || s.Contains("!"));
                case "√":
                    return (s.Contains("!"));
                case "!":
                    return false;
            }
            return false;
        }

        private static string CalcOp_AddSub(string s)
        {
            //分解 + - 运算符
            //下面几个 运算符分解函数都差不多，所以只注释该函数
            //si 当前位置，ss上一个运算符的位置
            int si = 0, ss = 0;
            //sl 左边，sr 右边，op 当前运算符
            string sl = "N", sr = "", op = "";
            //寻找运算符
            for (; si < s.Length; si++)
            {
                //找到了
                if (s[si] == '+' || s[si] == '-')
                {
                    if (s[si] == '+')
                    {
                        //运算符+
                        if (si > 0)
                        {
                            if (s[si - 1] == '+') op = "+";
                            //这里是为了防止前面加了-号（1-+1），所以这里设置为-（1-+1=1-1）
                            else if (s[si - 1] == '-') op = "-";
                            else op = "+";
                        }
                        else op = "+";
                    }
                    else if (s[si] == '-')
                    { 
                        //运算符-
                        if (si > 0)
                        {
                            //这里是为了防止前面加了-号，负负得正，正负得负
                            if (s[si - 1] == '+') op = "-";
                            else if (s[si - 1] == '-') op = "+";
                            else if (CalcIsOp(s[si - 1]))
                            {
                                //负号
                                continue;
                            }
                            else op = "-";
                        }
                        else op = "-";
                    }

                    //如果上一个运算符的位置-当前位置大于0
                    if (si - ss > 0)
                    {
                        //取出 上一个运算符 到 当前位置 这中间一段 字
                        //如 1+2-4  ss在+的位置，si在-的位置，就会取 +-之间的 2
                        //       ss  si
                        sr = s.Substring(ss, si - ss);
                        //如果前面有负号，特殊处理
                        if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                            sr = sr.Insert(0, "-");//加上负号
                        ss = si + 1;//设置上一个运算符的位置=当前位置+1
                        //如果取出来的这中间一段 字有更优先的运算符
                        //比如 1+2*5-4  ss 取的是2*5，有更优先的运算符 * ，所以就需要继续递归算出2*5
                        //           ss      si 
                        //当前运算符标识是 “+-”
                        if (CalcOp_HasMoreAdvancedOp(sr, "+-"))
                            sr = CalcCore(sr);//继续递归算出结果，放到右边
                        //运算单个运算符 左边 =左边 与 右边 运算。
                        sl = CalcOpSingal(sl, sr, op);
                    }
                    else
                    {
                        /*if (s[si] == '-' && si == 0)
                        {
                            int xi = 1;
                            for (; xi < s.Length; xi++)
                            {
                                if (CalcIsNoNumChar(s[xi]))
                                    break;
                            }
                            if (xi - si != 0)
                            {
                                sr = s.Substring(1, xi - 1);
                                sl = CalcOpSingal("0", sr, "-");
                            }
                        }*/
                        ss = si + 1;
                    }
                }
                if (si == s.Length - 1 && ss < s.Length)
                {
                    //这里是为了处理最后一个数
                    //  1+2 搜索到最后一个 2 时最后做一次运算
                    //同上面
                    sr = s.Substring(ss, si + 1 - ss);
                    if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                        sr = sr.Insert(0, "-");
                    if (CalcOp_HasMoreAdvancedOp(sr, "+-"))
                        sr = CalcCore(sr);
                    sl = CalcOpSingal(sl, sr, op);
                }
            }
            return sl;
        }
        private static string CalcOp_Mod(string s)
        {
            //分解 % MOD 运算符
            int si = 0, ss = 0;
            string sl = "N", sr = "", op = "";
            for (; si < s.Length; si++)
            {
                if (s[si] == '%' || (si > 0 && si + 1 < s.Length && s[si - 1] == 'M' & s[si] == 'O' & s[si + 1] == 'D'))
                {
                    if (s[si] == '%')
                        op = "%";
                    else if (si > 0 && si + 1 < s.Length && s[si - 1] == 'M' & s[si] == 'O' & s[si + 1] == 'D')
                    {
                        op = "%";
                        ss++;
                    }
                    if (si - ss > 0)
                    {
                        sr = s.Substring(ss, si - ss);
                        if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                            sr = sr.Insert(0, "-");
                        ss = si + 1;
                        if (CalcOp_HasMoreAdvancedOp(sr, "%"))
                            sr = CalcCore(sr);
                        sl = CalcOpSingal(sl, sr, op);
                    }
                    else ss = si + 1;
                }
                if (si == s.Length - 1 && ss < s.Length)
                {
                    sr = s.Substring(ss, si + 1 - ss);
                    if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                        sr = sr.Insert(0, "-");
                    if (CalcOp_HasMoreAdvancedOp(sr, "%"))
                        sr = CalcCore(sr);
                    sl = CalcOpSingal(sl, sr, op);
                }
            }
            return sl;
        }
        private static string CalcOp_MultDiv(string s)
        {
            //分解 * × ÷ / 运算符
            int si = 0, ss = 0;
            string sl = "N", sr = "", op = "";
            for (; si < s.Length; si++)
            {
                if (s[si] == '*' || s[si] == '×' || s[si] == '÷' || s[si] == '/')
                {
                    if (s[si] == '*' || s[si] == '×')
                        op = "*";
                    else if (s[si] == '÷' || s[si] == '/')
                        op = "/";
                    if (si - ss > 0)
                    {
                        sr = s.Substring(ss, si - ss);
                        if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                            sr = sr.Insert(0, "-");
                        ss = si + 1;
                        if (CalcOp_HasMoreAdvancedOp(sr, "*/"))
                            sr = CalcCore(sr);
                        sl = CalcOpSingal(sl, sr, op);
                    }
                    else ss = si + 1;
                }
                if (si == s.Length - 1 && ss < s.Length)
                {
                    sr = s.Substring(ss, si + 1 - ss);
                    if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                        sr = sr.Insert(0, "-");
                    if (CalcOp_HasMoreAdvancedOp(sr, "*/"))
                        sr = CalcCore(sr);
                    sl = CalcOpSingal(sl, sr, op);
                }
            }
            return sl;
        }
        private static string CalcOp_Pow(string s)
        {
            //分解 ^ 运算符
            int si = 0, ss = 0;
            string sl = "N", sr = "", op = "";
            for (; si < s.Length; si++)
            {
                if (s[si] == '^')
                {
                    if (s[si] == '^')
                        op = "^";
                    if (si - ss > 0)
                    {
                        sr = s.Substring(ss, si - ss);
                        if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                            sr = sr.Insert(0, "-");
                        ss = si + 1;
                        if (CalcOp_HasMoreAdvancedOp(sr, "^"))
                            sr = CalcCore(sr);
                        sl = CalcOpSingal(sl, sr, op);
                    }
                    else ss = si + 1;
                }
                if (si == s.Length - 1 && ss < s.Length)
                {
                    sr = s.Substring(ss, si + 1 - ss);
                    if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                        sr = sr.Insert(0, "-");
                    if (CalcOp_HasMoreAdvancedOp(sr, "^"))
                        sr = CalcCore(sr);
                    sl = CalcOpSingal(sl, sr, op);
                }
            }
            return sl;
        }
        private static string CalcOp_Sqrt(string s)
        {
            //分解 √ 运算符
            int si = 0, ss = 0;
            string sl = "N", sr = "", op = "";
            for (; si < s.Length; si++)
            {
                if (s[si] == '√')
                {
                    if (s[si] == '√')
                        op = "√";
                    int ix = si;
                    for (; ix < s.Length; ix++)
                        if (CalcIsOp(s[ix])) break;
                    if (ix - si > 0)
                    {
                        sr = s.Substring(ss, si - ss);
                        if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                            sr = sr.Insert(0, "-");
                        ss = si + 1;
                        if (CalcOp_HasMoreAdvancedOp(sr, "√"))
                            sr = CalcCore(sr);
                        sl = CalcOpSingal(sl, sr, op);
                    }
                    else ss = si + 1;
                }
                if (si == s.Length - 1 && ss < s.Length)
                {
                    sr = s.Substring(ss, si + 1 - ss);
                    if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                        sr = sr.Insert(0, "-");
                    if (CalcOp_HasMoreAdvancedOp(sr, "√"))
                        sr = CalcCore(sr);
                    sl = CalcOpSingal(sl, sr, op);
                }
            }
            return sl;
        }
        private static string CalcOp_Factorial(string s)
        {
            //分解 ! 运算符
            int si = 0, ss = 0;
            string sl = "N", sr = "", op = "";
            for (; si < s.Length; si++)
            {
                if (s[si] == '!')
                {
                    if (s[si] == '!')
                        op = "!";
                    if (si - ss > 0)
                    {
                        sr = s.Substring(ss, si - ss);
                        if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                            sr = sr.Insert(0, "-");
                        ss = si + 1;
                        if (CalcOp_HasMoreAdvancedOp(sr, "!"))
                            sr = CalcCore(sr);
                        sl = CalcOpSingal(sl, sr, op);
                    }
                    else ss = si + 1;
                }
                if (si == s.Length - 1 && ss < s.Length)
                {
                    sr = s.Substring(ss, si + 1 - ss);
                    if (ss > 1 && s[ss - 1] == '-' && CalcIsOp(s[ss - 2]))
                        sr = sr.Insert(0, "-");
                    if (CalcOp_HasMoreAdvancedOp(sr, "!"))
                        sr = CalcCore(sr);
                    sl = CalcOpSingal(sl, sr, op);
                }
            }
            return sl;
        }
    }
}
