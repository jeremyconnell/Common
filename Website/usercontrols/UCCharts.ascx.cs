using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using Framework;

//Reference: http://code.google.com/apis/visualization/documentation/gallery/imagelinechart.html
//           http://code.google.com/apis/visualization/documentation/gallery/linechart.html
public partial class Dashboard_Controls_UCCharts : System.Web.UI.UserControl
{
    #region Enums
    public enum ELegend
    {
        none,
        right,
        left,
        top,
        bottom,
        label
    }
    #endregion

    #region Constants
    public static List<string> PAIR_OF_COLORS = new List<string>(new string[] { "#4594bd", "#6d6d6d" });
    #endregion

    #region Members
    private int _width = 800;//650;
    private int _height = 400; //340;
    private int _areaWidth = 80;
    private int _areaHeight = 80;
    private ELegend _legend = ELegend.none;
    private bool _interpolateNulls = true;
    #endregion

    #region Properties
    public int Width { get { return _width; } set { _width = value; } }
    public int Height { get { return _height; } set { _height = value; } }
    public int AreaWidth { get { return _areaWidth; } set { _areaWidth = value; } }
    public int AreaHeight { get { return _areaHeight; } set { _areaHeight = value; } }
    public ELegend Legend { get { return _legend; } set { _legend = value; } }
    public bool InterpolateNulls { get { return _interpolateNulls; } set { _interpolateNulls = value; } }

    public bool IsFirstOne { get { return plhFirstOneOnly.Visible; } set { plhFirstOneOnly.Visible = value; } }

    public int Number { get { return Math.Abs(ClientID.GetHashCode()); } }
    public string DivId { get { return string.Concat("chart_div_", Number); } }
    #endregion

    #region Chart Helper Methods

    //Html id
    public string GetDiv { get { return string.Concat("document.getElementById('", DivId, "')"); } }

    //Presentation
    public void Layout(StringBuilder sb) { Layout(sb, string.Empty); }
    public void Layout(StringBuilder sb, string title) { Layout(sb, title, Width, Height); }
    public void Layout(StringBuilder sb, string title, List<string> colors) { Layout(sb, title, Width, Height, colors); }
    public void Layout(StringBuilder sb, string title, int width, int height) { Layout(sb, title, width, height, null); }
    public void Layout(StringBuilder sb, string title, int width, int height, List<string> colors)
    {
        //Colors bit
        StringBuilder sb1 = new StringBuilder();
        if (null != colors && colors.Count > 0)
        {
            foreach (string i in colors)
            {
                if (sb1.Length > 0)
                    sb1.Append(",");
                sb1.Append("'").Append(i).Append("'");
            }
            sb1.Insert(0, ", colors: new Array(");
            sb1.Append(")");
        } 

        //Draw with params
        sb.Append("chart.draw(data, { chartArea:{width:\"").Append(_areaWidth).Append("%\",height:\"").Append(_areaHeight).Append("%\"} ,  width:").Append(width).Append(" , height:").Append(height).Append(", is3D:true, title:'").Append(title.Replace("'", "\\'")).Append("', legend:'").Append(_legend).Append("'").Append(sb1).AppendLine(", interpolateNulls:").Append(InterpolateNulls.ToString().ToLower()).Append(" });");
    }

    //Schema def.
    private void AddColumnString(StringBuilder sb, string name)
    {
        sb.Append("data.addColumn('string', '").Append(name).AppendLine("');");
    }
    private void AddColumnNumber(StringBuilder sb, string name)
    {
        sb.Append("data.addColumn('number', '").Append(name).AppendLine("');");
    }
    private void AddRows(StringBuilder sb, int rows)
    {
        sb.Append("data.addRows(").Append(rows).AppendLine(");");
    }


    //Data def.
    private void AddData(StringBuilder sb, int row, int column, double data)
    {
        AddData(sb, row, column).Append(", ").Append(data).AppendLine(");");
    }
    private void AddData(StringBuilder sb, int row, int column, long data)
    {
        AddData(sb, row, column).Append(", ").Append(data).AppendLine(");");
    }
    private void AddData(StringBuilder sb, int row, int column, decimal data)
    {
        AddData(sb, row, column).Append(", ").Append(data).AppendLine(");");
    }
    private void AddData(StringBuilder sb, int row, int column, int data)
    {
        AddData(sb, row, column).Append(", ").Append(data).AppendLine(");");
    }
    private void AddData(StringBuilder sb, int row, int column, string data)
    {
        data = data.Replace("'", @"\'");
        AddData(sb, row, column).Append(", '").Append(data).AppendLine("');");
    }
    private StringBuilder AddData(StringBuilder sb, int row, int column)
    {
        return sb.Append("data.setValue(").Append(row).Append(", ").Append(column);
    }

    //Money Def
    private void AddMoneyLabel(StringBuilder sb, int row, int column, string label)
    {
        label = label.Replace("'", @"\'");
        AddMoney(sb, row, column).Append(", '").Append(label).AppendLine("');");
    }
    private void AddMoneyValue(StringBuilder sb, int row, int column, decimal amount)
    {
        if (amount == decimal.MinValue) amount = 0;
        AddMoney(sb, row, column).Append(", ").Append(Math.Round(amount)).Append(", '").Append(amount.ToString("C")).AppendLine("');");
    }
    private void AddMoneyValue(StringBuilder sb, int row, int column, int amount)
    {
        if (amount == int.MinValue) amount = 0;
        AddMoney(sb, row, column).Append(", ").Append(amount).Append(", '").Append(amount).AppendLine("');");
    }
    private StringBuilder AddMoney(StringBuilder sb, int row, int column)
    {
        return sb.Append("data.setCell(").Append(row).Append(", ").Append(column);
    }
    #endregion

    #region Chart Types
    private string Script_MoneyChart(string entity, CNameValueList data, string counting)
    {
        StringBuilder sb = new StringBuilder();

        //Limit width
        int count = data.Count;
        if (count > 5)
            count = 5;
        if (count == 0)
            this.Visible = false;

        //Data
        AddColumnString(sb, entity);
        AddColumnNumber(sb, counting);
        AddRows(sb, count);
        for (int i = 0; i < count; i++)
        {
            CNameValue nv = data[i];
            AddMoneyLabel(sb, i, 0, nv.Name);
            AddData(sb, i, 1, nv.Value);
        }

        //Presentation
        sb.Append("chart = new PilesOfMoney(").Append(GetDiv).AppendLine(");");
        Layout(sb, string.Empty);

        return sb.ToString();
    }
    private string Script_ScatterChart(string title, CNameValueList data, string xLabel, string yLabel, bool connnectDots)
    {

        StringBuilder sb = new StringBuilder();

        decimal minX = decimal.MaxValue;
        decimal minY = decimal.MaxValue;
        decimal maxX = decimal.MinValue;
        decimal maxY = decimal.MinValue;

        //Data
        AddColumnNumber(sb, xLabel);
        AddColumnNumber(sb, yLabel);
        AddRows(sb, data.Count);
        for (int i = 0; i < data.Count; i++)
        {
            CNameValue nv = data[i];
            decimal x = decimal.Parse(nv.Name);
            decimal y = (decimal)nv.Value;
            if (minX > x) minX = x;
            if (minY > y) minY = y;
            if (maxX < x) maxX = x;
            if (maxY < y) maxY = y;
            AddData(sb, i, 0, x);
            AddData(sb, i, 1, y);
        }

        //Presentation
        sb.Append("chart = new google.visualization.ScatterChart(").Append(GetDiv).AppendLine(");");
        //Layout(sb, title);
        sb.Append("chart.draw(data, { ").Append(connnectDots ? " lineWidth:1, curveType: 'function', " : " pointSize:1,").Append("width: ").Append(Width).Append(", height: ").Append(Height).Append(", title: '").Append(title).Append("', hAxis: { title: '").Append(xLabel).Append("', minValue: ").Append(minX).Append(", maxValue: ").Append(maxX).Append(" }, vAxis: { title: '").Append(yLabel).Append("', minValue: ").Append(minY).Append(", maxValue: ").Append(maxY).Append(" }, legend: 'none' });");


        return sb.ToString();
    }
    private string Script_BarOrLineChart(string entity, CNameValueList data, string counting, bool lineChart)
    {
        StringBuilder sb = new StringBuilder();
        if (lineChart)
            sb.Append("var chart = new google.visualization.LineChart(").Append(GetDiv).AppendLine(");");
        else
            sb.Append("var chart = new google.visualization.ColumnChart(").Append(GetDiv).AppendLine(");");

        //Limit width
        int count = data.Count;
        if (count > 5000)
            count = 5000;
        if (count == 0)
            this.Visible = false;

        //Data
        AddColumnString(sb, entity);
        if (data.Count > 0 && data[0].Value is IEnumerable)
        {
            foreach (string i in CUtilities.StringToListStr(counting))
                AddColumnNumber(sb, i);
        }
        else
            AddColumnNumber(sb, counting);

        AddRows(sb, count);
        for (int i = 0; i < count; i++)
        {
            CNameValue nv = data[i];
            AddData(sb, i, 0, nv.Name);
            AddData(sb, i, 1, nv.Value);
        }

        //Presentation
        Layout(sb, entity);

        return sb.ToString();
    }
    private string Script_PieChart(string entity, CNameValueList data, string counting)
    {
        StringBuilder sb = new StringBuilder();

        //Data
        AddColumnString(sb, entity);
        AddColumnNumber(sb, counting);
        AddRows(sb, data.Count);
        for (int i = 0; i < data.Count; i++)
        {
            CNameValue nv = data[i];
            AddMoneyLabel(sb, i, 0, nv.Name);
            AddData(sb, i, 1, nv.Value);
        }

        //Presentation
        sb.Append("chart = new google.visualization.PieChart(").Append(GetDiv).AppendLine(");");
        _legend = ELegend.label;
        Layout(sb, entity);

        return sb.ToString();
    }
    private void AddData(StringBuilder sb, int columnIndex, int seriesIndex, object obj)
    {
        if (obj is int)
            AddData(sb, columnIndex, seriesIndex, int.MinValue == (int)obj ? 0 : (int)obj);
        else if (obj is long)
            AddData(sb, columnIndex, seriesIndex, long.MinValue == (long)obj ? 0 : (long)obj);
        else if (obj is string)
            AddData(sb, columnIndex, seriesIndex, CTextbox.GetMoney((string)obj));
        else if (obj is double)
        {
            if (!double.IsNaN((double)obj))
                AddData(sb, columnIndex, seriesIndex, double.IsNaN((double)obj) ? (double)0 : (double)obj);
        }
        else if (obj is IEnumerable)
        {
            int index = 0;
            IEnumerable e = ((IEnumerable)obj);
            foreach (object i in e)
                AddData(sb, columnIndex, seriesIndex + index++, i);
        }
        else
            AddData(sb, columnIndex, seriesIndex, (null == obj || decimal.MinValue == (decimal)obj) ? (decimal)0 : (decimal)obj);
    }
    #endregion

    #region Interface Methods

    public void ScatterChart(CNameValueList nv, string title, string x, string y, bool connectDots)
    {
        string script = Script_ScatterChart(title, nv, x, y, connectDots);
        SetChart(script);
    }

    //Column labels are only applicable to multi-series for line/bar
    public void BarChart(CNameValueList nv, string title) { BarChart(nv, title, string.Empty); }
    public void LineChart(CNameValueList nv, string title) { LineChart(nv, title, string.Empty); }
    public void BarOrLineChart(CNameValueList nv, string title, bool lineChart) { BarOrLineChart(nv, title, string.Empty, lineChart); }

    public void BarChart(CNameValueList nv, string title, string column) { BarOrLineChart(nv, title, column, false); }
    public void LineChart(CNameValueList nv, string title, string column) { BarOrLineChart(nv, title, column, true); }
    public void BarOrLineChart(CNameValueList nv, string title, string column, bool lineChart)
    {
        //nv = LimitTo(nv, 50);
        string script = Script_BarOrLineChart(title, nv, column, lineChart);
        SetChart(script);
    }
    public void PieChart(CNameValueList nv, string title)
    {
        nv = LimitTo(nv, 25);
        string script = Script_PieChart(title, nv, "counting");
        SetChart(script);
    }
    private void SetChart(string script)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div id=").Append(DivId).AppendLine("></div>");
        sb.AppendLine("<script type=\"text/javascript\">");
        sb.Append("\tgoogle.setOnLoadCallback(drawChart").Append(Number).AppendLine(");");
        sb.Append("\tfunction drawChart").Append(Number).AppendLine("()");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tvar data = new google.visualization.DataTable();");
        sb.AppendLine(script);
        sb.AppendLine("\t}");
        sb.AppendLine("</script>");
        lit.Text = sb.ToString();
    }
    private CNameValueList LimitTo(CNameValueList nv, int limit)
    {
        if (nv.Count <= limit)
            return nv;
        nv = new CNameValueList(nv);
        List<CNameValue> top = nv.GetRange(0, limit);
        long other = 0;
        for (int i = limit; i < nv.Count; i++)
            other += (long)nv[i].Value;
        nv.Clear();
        nv.AddRange(top);
        nv.Add(string.Concat("Other (limit ", limit, ")"), other);
        return nv;
    }

    #endregion


}
