using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice
{
    public partial class Form1 : Form
    {
        public enum OperateProcess { 
            LeftNum,
            RightNum,
            Result
        }

        public double leftNum = 0;      //第一操作数
        public double rightNum;          //第二操作数
        public double jsResult;          //结果值
        public string operateSign;          //运算符号
        public OperateProcess operateProcess = OperateProcess.LeftNum;    //检测是否为第一操作数

        public Form1()
        {
            InitializeComponent();
            ButtonAttribute.FoutAttribute(this);            
        }
        /// <summary>
        /// 文本框限制条件,ASCII(48为数字0,57为数字9,8为退格,45为符号"-",46为符号".")
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46)
            {
                e.Handled = true;           //文本框只可以输入数字0~9和退格键和符号"-"和符号"."
            }
            if (e.KeyChar == 45 && ((TextBox)sender).Text != "")
            {
                e.Handled = true;           //文本框不为空时不能输入符号"-"
            }
            if (e.KeyChar == 46 && ((TextBox)sender).Text == "")
            {
                e.Handled = true;           //文本框为空时不能输入符号"."
            }
            if (e.KeyChar == 46 && ((TextBox)sender).Text.IndexOf(".") != -1)
            {
                e.Handled = true;           //文本框只允许输入一次符号"."
            }
            if (((TextBox)sender).Text == "-" && e.KeyChar == 46)
            {
                e.Handled = true;           //文本框第一位是符号"-"时第二位不允许输入符号"."
            }
            if (((TextBox)sender).Text == "0" && e.KeyChar != 46 && e.KeyChar != 8)
            {
                e.Handled = true;           //文本框第一位是"0"时第二位必须为符号"."并且可以退格
            }
            if (((TextBox)sender).Text == "-0" && e.KeyChar != 46 && e.KeyChar != 8)
            {
                e.Handled = true;           //文本框第一位是符号"-"第二位是0,第三位必须是符号"."并且可以退格
            }
        }
        private void btn0_Click(object sender, EventArgs e)
        {
            //就比如这里
            //所有的数字都调用该方法,具体思路如下
            Button b = (Button)sender;
            //0.这里其实还有场景呢
            switch (operateProcess)
            { 
                //1.若正在编辑,则将后面输入的值累加到第一操作数后
                case OperateProcess.LeftNum:
                    tbOperateNumEditing.Text += b.Text;
                    leftNum = double.Parse(tbOperateNumEditing.Text);
                    break;
                //2.否则,将后面输入的值依序累加到第二操作数
                case OperateProcess.RightNum:
                    tbOperateNumEditing.Text += b.Text;
                    rightNum = double.Parse(tbOperateNumEditing.Text);
                    break;
                //3.若现在是点完=号出来结果了,这次再点击数字,应该是什么操作呢?
                case OperateProcess.Result:
                    tbOperateNumEditing.Text = b.Text;
                    rightNum = double.Parse(tbOperateNumEditing.Text);
                    operateProcess = OperateProcess.RightNum;
                    break;
            }


            //1.检测第一操作数(左操作数)是否正在编辑中
     
        }
        

        private void btnac_Click(object sender, EventArgs e)
        {
            tbOperateNumEditing.Text = "";
            leftNum = 0;
            rightNum = 0;
            jsResult = 0;
            operateProcess = OperateProcess.LeftNum;
        }
        private void btnbackspace_Click(object sender, EventArgs e)
        {
            //退格键的场景分析

            //首先先执行框中数据变更
            //
            if (!string.IsNullOrEmpty(this.tbOperateNumEditing.Text))
            //2.有值的情况下
            {
                //只有在有值并且在第一第二操作数编辑的情况下执行退格
                if (operateProcess != OperateProcess.Result)
                    //tbOperateNumEditing.Text = tbOperateNumEditing.Text.
                    //    Substring(0, tbOperateNumEditing.Text.Length - 1);

                    tbOperateNumEditing.Text = tbOperateNumEditing.Text.Remove(tbOperateNumEditing.Text.Length - 1);

                //tbOperateNumEditing.Text由于出现变更,赋值时需要小心在框中无值的情况
                switch (operateProcess)
                {
                    //2.1 当在第一操作数情况下
                    case OperateProcess.LeftNum:
                        leftNum = string.IsNullOrEmpty(this.tbOperateNumEditing.Text) ? 0 :
                            double.Parse(tbOperateNumEditing.Text);
                        break;
                    //2.2 当在第二操作数情况下
                    case OperateProcess.RightNum:
                        rightNum = string.IsNullOrEmpty(this.tbOperateNumEditing.Text) ? 0 :
                            double.Parse(tbOperateNumEditing.Text);
                        break;
                    //2.3 当在结果的情况下
                    case OperateProcess.Result:
                        break;

                }

            }
            else
            { 
                //1.在运算框里没值的情况下
            
            }



            //if(tbOperateNumEditing.Text.Length > 0)
            //{
            //    if (operateProcess == OperateProcess.LeftNum)
            //    {
            //        tbOperateNumEditing.Text = tbOperateNumEditing.Text.Substring(0, tbOperateNumEditing.Text.Length - 1);
            //        leftNum = double.Parse(tbOperateNumEditing.Text);
            //    }
            //    else if (operateProcess == OperateProcess.RightNum)
            //    {
            //        tbOperateNumEditing.Text = tbOperateNumEditing.Text.Substring(0, tbOperateNumEditing.Text.Length - 1);
            //        rightNum = double.Parse(tbOperateNumEditing.Text);
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}


        }
        private void btndot_Click(object sender, EventArgs e)
        {

            if (tbOperateNumEditing.Text == "")
            {
                tbOperateNumEditing.Text = "0.";
            }           
            if (tbOperateNumEditing.Text == "-")
            {
                return;
            }
            if (tbOperateNumEditing.Text.IndexOf(".") >= 0)
            {
                return;
            }
            else
            {
                tbOperateNumEditing.Text = tbOperateNumEditing.Text + ".";
            }

        }

        private void btnadd_Click(object sender, EventArgs e)
        {

            //加号场景,四种吧?
            //1.计算器加载完后开始就点击(最开始点击)


            switch (operateProcess)
            { 
                //2.输入完左操作数后点击
                case OperateProcess.LeftNum:
                    operateSign = "+";
                    tbOperateNumEditing.Text = "";
                    operateProcess = OperateProcess.RightNum;

                    break;
                //3.输入完右操作数后点击
                case OperateProcess.RightNum:
                    switch (operateSign)
                    {
                        case "+":
                            jsResult = leftNum + rightNum;                        
                            break;
                    }
                    tbOperateNumEditing.Text = jsResult.ToString();
                    leftNum = jsResult;
                    operateProcess = OperateProcess.Result;
                    break;
                //4.多次重复点击
                case OperateProcess.Result:

                    break;
            }
            
        }

        private void btnminus_Click(object sender, EventArgs e)
        {
            if(tbOperateNumEditing.Text == "")
            {
                tbOperateNumEditing.Text = "-";
            }
            else
            {
                operateSign = "-";
                tbOperateNumEditing.Text = "";
                //isEditingLeftNum = false;
            }
                        
        }

        private void btnmutiplication_Click(object sender, EventArgs e)
        {
            operateSign = "*";
            tbOperateNumEditing.Text = "";
            //isEditingLeftNum = false;
        }

        private void btndivide_Click(object sender, EventArgs e)
        {
            operateSign = "/";
            tbOperateNumEditing.Text = "";
            //isEditingLeftNum = false;

        }

        private void btnequal_Click(object sender, EventArgs e)
        {
            operateProcess = OperateProcess.Result;
            switch (operateSign)
            {
                case "+":
                    jsResult = leftNum + rightNum;
                    break;
                case "-":
                    jsResult = leftNum - rightNum;
                    break;
                case "*":
                    jsResult = leftNum * rightNum;
                    break;
                case "/":
                    jsResult = leftNum / rightNum;
                    break;                    
            }
            leftNum = jsResult;
            tbOperateNumEditing.Text = jsResult.ToString();
        }

        

        

        //private void Form1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    string code = e.KeyCode.ToString();
        //    int currnum = 0;
        //    bool numparse = int.TryParse(code.Replace("D", ""), out currnum);

        //    //只处理数字按键
        //    if (numparse)
        //    {
        //        Console.WriteLine(code);
        //        string btnname = String.Format("btn{0}", currnum);
        //        //根据数字去找到相应的数字按钮对象
        //        Button btn = ((Button)(this.groupBox1.Controls.Find(btnname, false)[0]));
        //        btn.Focus();

        //        //执行这个数字按钮的click事件
        //        callButtonEvent(btn, "OnClick");
        //    }
        //}
        //private void callButtonEvent(Button btn, string EventName)
        //{
        //    //建立一个类型      
        //    Type t = typeof(Button);
        //    //参数对象      
        //    object[] p = new object[1];
        //    //产生方法      
        //    MethodInfo m = t.GetMethod(EventName, BindingFlags.NonPublic | BindingFlags.Instance);
        //    //参数赋值。传入函数      
        //    //获得参数资料  
        //    ParameterInfo[] para = m.GetParameters();
        //    //根据参数的名字，拿参数的空值。  
        //    p[0] = Type.GetType(para[0].ParameterType.BaseType.FullName).GetProperty("Empty");
        //    //调用      
        //    m.Invoke(btn, p);
        //    return;
        //}



    }
}
