<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="Rapid.Index.Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        .mkTitle
        {
            font-size: 17px;
            font-weight: bold;
            color: Green;
        }
    </style>

    <script type="text/javascript">
        function Start() {
            document.getElementById("lbResult").innerhtml = "系统数据修复中......";
            return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; font-size: 25px;">
        系统公告
    </div>
    <div style="text-align: left;">
        <p>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;欢迎使用瑞普迪Erp系统 V1.0，本系统运行环境为IE9。系统分为销售管理、采购管理、生产管理、库房管理和财务管理五个模块。
        </p>
        <p>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 以下为模块在使用过程中需要注意的地方，在使用过程中如若遇到问题请及时与技术人员联系。
        </p>
    </div>
    <div style="text-align: left; margin-left: 60px;">
        <p class="mkTitle" style="color: Red;">
            注意事项</p>
        <div>
            <div style="margin-left: 40px;">
                <div style="color: Blue;">
                    Excel导入规则</div>
                <div style="color: Red; margin-left: 45px;">
                    导入Excel时请先对Excel文件进行此操作：点左上方的行列标号的交叉处选中整表，复制》选择性粘贴》数值。
                    <br />
                    <p style="color: Black;">
                        说明：此操作替换Excel文件中的公式为数值，如果不进行此项操作，Excel将会导入失败。</p>
                    所有导入的Excel文件中的日期格式统一改成“yyyy-MM-dd”，例如：2014年1月9号，改成"2014-01-09"。具体操作步骤：选择单元格右键》设置单元格格式》分类选择：自定义，类型输入："yyyy-mm-dd;@"》确定
                    <br />
                    导入Excel前请去除所有格式。例如：去除固定网格、筛选等格式。
                    <br />
                    所有Excel导入模板以系统首页展示的导入模板为最新标准。请严格按照模板内容进行内容填充，严禁修改模板表头内容。
                    <br />
                    <p style="color: Black;">
                        请严格遵守以上Excel导入规则，否则Excel有可能导入失败。
                        <br />
                        在导入Excel过程中如果系统出现报错红色页面，请检查Excel是否符合以上导入规则，修正后再次进行导入即可。</p>
                    <br />
                </div>
                系统所有的编号请一律输入字母或数字，请勿输入中文。没有版本号的产成品，版本号统一填写成“WU”。<br />
                BOM的单位:mm【请勿使用中文“毫米”】<br />
            </div>
        </div>
        <p class="mkTitle" style="color: Red;">
            名词解释</p>
        <div>
            <div style="margin-left: 40px;">
                产成品编号（产品编号）：瑞普迪自己的产品编号<br />
                客户产成品编号：客户产品图纸号。<br />
                原材料编号：瑞普迪自己的原材料编号。<br />
                供应商物料编号：供应商型号。<br />
                客户物料编号：客户物料号 。</div>
        </div>
        <p class="mkTitle">
            Excel导入模板</p>
        <div>
            销售管理</div>
        <br />
        <div>
            <a target="_blank" href="../Text/销售订单通用导入模板.xls">销售订单通用导入模板（含加工贸易）.xls</a>
            <%--            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/加工销售订单导入模板.xls">加工销售订单导入模板.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/贸易销售订单导入模板.xls">贸易销售订单导入模板.xls</a>--%>&nbsp;&nbsp;&nbsp;&nbsp;<a
                target="_blank" href="../Text/客户信息导入模板.xls"> 客户信息导入模板.xls</a> &nbsp;&nbsp;&nbsp;&nbsp;<a
                    target="_blank" href="../Text/项目信息导入模板.xls">项目信息导入模板.xls</a> &nbsp;&nbsp;&nbsp;&nbsp;<a
                        target="_blank" href="../Text/贸易报价单导入模板.xls"> 贸易报价单导入模板.xls</a>&nbsp;&nbsp;&nbsp;&nbsp;
            <a target="_blank" href="../Text/加工报价单导入模板.xls">加工报价单导入摸版.xls</a>
        </div>
        <br />
        <div>
            采购管理</div>
        <br />
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/供应商信息导入模板.xls">供应商信息导入模板.xls</a>&nbsp;&nbsp;&nbsp;&nbsp;
            <a target="_blank" href="../Text/采购订单导入模板.xls">采购订单导入模板.xls</a></div>
        <br />
        <div>
            生产管理</div>
        <br />
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp;<a href="../Text/原材料导入模板.xls" target="_blank">原材料导入模板.xls</a>&nbsp;&nbsp;&nbsp;&nbsp;
            <a target="_blank" href="../Text/考试成绩上报模板.xls">考试成绩上报模板.xls</a> &nbsp;&nbsp;&nbsp;&nbsp;
            <a target="_blank" href="../Text/考勤上报模板.xls">考勤上报模板.xls</a> &nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/不合格品上报模版.xls">不合格品上报模版.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/产成品基本信息导入模板.xls">产成品基本信息导入模板.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/二级BOM导入模板（不是包的）.xls">二级BOM导入模板（不是包的）.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/三级BOM导入模板（是包的）.xls">三级BOM导入模板（是包的）.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/工序工时导入模板.xls">工序工时导入模板.xls</a>
        </div>
        <br />
        <div>
            库房管理</div>
        <br />
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/原材料报废上报模板.xls">原材料报废上报模板.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/原材料库存导入模板.xls">原材料库存导入模板.xls</a>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/产成品库存导入模板.xls">产成品库存导入模板.xls</a>
        </div>
        <div>
            财务管理</div>
        <br />
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="../Text/发票登记表.xls">发票登记导入模板.xls</a>
        </div>
        <p class="mkTitle">
            销售管理</p>
        <div>
            <div style="margin-left: 40px;">
                <label style="color: #7700BB;">
                    订单号规则</label>
                ：除临时订单号系统自动生成外，其它类型订单的订单号手动填写。
                <br />
                审核：产生审核人和审核时间，订单审核后不能进行增删改操作。
                <br />
            </div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;送货单
            <div style="margin-left: 40px;">
                产成品送货单和原材料送货单通用一种格式。<br />
                汇总：相同客户才能进行汇总。<br />
                <br />
                确认：产生审核人和审核时间，产生应收账款，订单审核后不能进行增删改操作。
            </div>
        </div>
        <p class="mkTitle">
            采购管理</p>
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp; 采购订单
            <br />
            <div style="margin-left: 40px;">
                <label style="color: #7700BB;">
                    订单号规则</label>
                ：如果没有输入订单号系统将自动生成，反之则使用输入的订单号。
                <br />
            </div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;MRP运算
            <div style="margin-left: 40px;">
                <label style="color: blue;">
                    公式</label>
                ：平衡结果=（库存数量+采购在途数量）-订单需求数-安全库存数量， 大于等于0时，不需采购；小于0时需要采购。产生供需平衡表。
                <br />
            </div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;供应商信息
            <div style="margin-left: 40px;">
                账期：采购订单的付款方式为货到付款时，从此处取账期。
                <br />
                预付百分比：采购订单的付款方式为预付部分货款时，从此处取百分比。
                <br />
            </div>
        </div>
        <p class="mkTitle">
            生产管理</p>
        <div>
            <span style="color: Red;">&nbsp;&nbsp;&nbsp;&nbsp; 注意：原材料类型信息、基础工序信息为基础信息表请勿删除。</span>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; 产成品信息
            <br />
            <div style="margin-left: 40px;">
                额定工时=产品工序工时的总和。<br />
                成本价=bom原材料采购价的总和。<br />
                <br />
            </div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;开工单列表
            <div style="margin-left: 40px;">
                开工单（总表）主表
                <div style="margin-left: 60px;">
                    <br />
                    输入：<br />
                    实际开始时间、实际结束时间、计划开始时间、计划结束时间<br />
                    计算：<br />
                    额定总工时=开工单（总表）明细合计工时汇总。<br />
                    人数=开工单（分表）人数汇总。<br />
                    目标完成工时=开工单（分表）明细汇总目标完成工时。<br />
                    实际总工时=开工单（分表）明细汇总实际总工时。<br />
                    实际完成工时=开工单（分表）明细汇总实际完成工时。<br />
                </div>
                <br />
                开工单（分表）主表
                <div style="margin-left: 60px;">
                    <br />
                    输入：<br />
                    实际开始时间、实际结束时间。
                    <br />
                    计算：<br />
                    实际总工时=实际结束时间-实际开始时间。<br />
                    额定总工时=开工单（分表）明细按班组汇总合计工时。<br />
                    目标完成工时=额定总工时/人数。<br />
                    实际完成工时=实际总工时/人数。
                </div>
                <br />
            </div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;生成开工单
            <div style="margin-left: 40px;">
                分为小组开工单和工序开工单，小组开工单可拆分。<br />
            </div>
        </div>
        <p class="mkTitle">
            库房管理</p>
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp; 原材料出入库审核：
            <br />
            <div style="margin-left: 40px;">
                ①产生审核人和审核时间。
                <br />
                ②同步更新库存数量。
                <br />
                ③类型为采购入库的产生应付。
                <br />
                ④类型为采购入库的同步更新采购订单的已交货数量、未交货数量、订单完成状态。</div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;产品出入库审核：
            <div style="margin-left: 40px;">
                ①产生审核人和审核时间。
                <br />
                ②同步更新库存数量。
            </div>
        </div>
        <%--  <p class="mkTitle">
            财务管理</p>
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp;应付产生来源（采购订单）：
            <br />
            <div style="margin-left: 40px;">
                ①付款方式为预付全款和预付部分【从供应商的预付百分比中匹配】的采购订单，审核后立即产生应付。<br />
                ②付款方式为预付部分和货到付款【账期为0天的】的采购订单剩余应付在原材料采购入库审核后产生。
                <br />
                ③付款方式为货到付款【账期不为0天的】和月结的采购订单通过数据库定时作业产生。</div>
        </div>
        <div style="margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;应收产生来源（销售订单）：
            <br />
            <div style="margin-left: 40px;">
                ①收款方式为预收全款、预收部分以及货到收款（账期为0天的）的销售订单在送货单确认后产生应收。<br />
                ③收款款方式为货到收款款【账期不为0天的】和月结的销售订单通过数据库定时作业产生。</div>
        </div>--%>
    </div>
    <div style="text-align: left; display: <%=userId.Equals("sysAdmin")?"inline":"none"%>;">
        <p>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;本系统提供错误数据自动修复功能，以“超级管理员”身份进入即可操作。
        </p>
        <p>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;修复数据内容包括：销售订单的已交、未交、状态、开工单在制品数量。
        </p>
        <p>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <asp:Button ID="Button1" Text="修复系统错误数据" runat="server" OnClick="Button1_Click" OnClientClick="return Start()" />
            <asp:Label runat="server" ID="lbResult" ForeColor="Red"> </asp:Label>
        </p>
    </div>
    </form>
</body>
</html>
