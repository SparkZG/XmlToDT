using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToDT
{
    class Models
    {
        public static string[] groupName = new string[] { "basicConfig", "teleMeterNode", "teleControl", "teleSignalGB", "teleSignal", "modeText", "int_para", "bit_para", "teleMeter" };

        public static string basicConfig_dc = "包含“协议名称”、“协议版本号”和“刷新方式”等基本信息，此表一般不需要更改(注：列basicConfig_Id为“基本定义”索引，必填！此处恒为“0”)";

        public static string teleMeterNode_dc = "包含“电芯定义”、“温度定义”和其他基本信息，列teleMeter_Block_Id为“遥测定义”索引，必填！其中“0”代表“电芯模块”，“1”代表“电池温度”，“2”和“3”代表其他基本信息(注：点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string teleControl_dc = "包含“放电控制”、“充电控制”和“限流控制”等控制命令(注：列teleControl_Group_Id为“遥控定义”索引，必填！此处恒为“0”。点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string modeText_dc = "包含“放电”、“充电”和“浮充”等工作模式(注：列teleSignal_block_Id为“遥信定义”索引，必填！此处恒为“4”。点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string teleSignal_dc = "包含“遥信定义位信号”和“历史告警定义”信息(注：列teleSignal_block_Id为“遥信定义”索引，列warnHistory_Id为“历史告警定义”索引，如若想增加“遥信位信号”定义，将列teleSignal_block_Id值置为“3”，列warnHistory_Id不填！同理要是增加“历史告警定义”,列warnHistory_Id置为“0”，列teleSignal_block_Id不填。点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string teleSignalGB_dc = "包含“电压”、“温度”和“其他”遥信定义(注：列teleSignal_block_Id为“遥信定义”索引，必填！其中“0”代表“电压模块”，“1”代表“温度模块”，“2”代表其他“遥信定义”。点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string int_para_dc = "包含“整形参数定义”和“校准参数定义”信息(注：列int_para_Group_Id为“整形参数定义”索引，列adjust_para_Group_Id为“校准参数定义”索引，如若想增加“整形参数定义”定义，将列int_para_Group_Id值置为“0”，列adjust_para_Group_Id不填！同理要是增加“历史告警定义”,列adjust_para_Group_Id置为“0”，列int_para_Group_Id不填。点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string bit_para_dc = "包含“单体过压告警”、“单体过压保护”和“单体欠压告警”等位参数定义(注：列bit_para_Group_Id为“位参数定义”索引，必填！此处恒为“0”。点击表格左上角“+”和“-”图标进行插入和删除行)";

        public static string teleMeter_dc = "包含“充放电流”、“电池总压”和“剩余容量”等历史记录定义(注：列warnHistory_Id为“历史告警定义”索引，必填！此处恒为“0”。若无teleMeter定义 表明历史记录的遥测部分完全依照前面遥测部分定义。点击表格左上角“+”和“-”图标进行插入和删除行)";
    }
}
