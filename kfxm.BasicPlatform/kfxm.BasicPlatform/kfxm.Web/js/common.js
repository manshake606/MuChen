

var relationshipWinName = "";
var hbesWinCloseType = "error";

function openWin(title, url, width, heigth, okRunMethodName) {
    if (width < 400) {
        width = 400;
    }
    if (heigth < 200) {
        heigth = 200;
    }
    var iframeWindowName = window.name;
    parent.layer.open({
        title: title,
        type: 2,
        area: ['' + width + 'px', '' + heigth + 'px'],
        fixed: false, //不固定
        shadeClose:true,
        closeBtn: 1,
        content: url,
        scrollbar:'true',
        yes: function (index, layero)
        {
            alert(index);
            var method = "save";
            if (okRunMethodName != null && okRunMethodName != undefined && $.trim(okRunMethodName) != "") 
            {
                method = okRunMethodName;
            }
            //var body = layer.getChildFrame('body', index);
           

        },
        end: function () {

         
            if (parent.hbesWinCloseType == "ok")
            {
                parent.hbesWinCloseType = "error";
                if (okRunMethodName != null && okRunMethodName != undefined && $.trim(okRunMethodName) != "")
                {
                    excuteFn(okRunMethodName);
                }
            }
        }
    });
}
function excuteFn(functionName) {
    var func = eval(functionName);
    new func();
}
function closeWin() {
    
    var pindex = parent.layer.getFrameIndex(window.name);
    parent.layer.close(pindex);
}
function getFormMap(id) {

    var controls = $("#" + id + " .form-control");
    var resultMap = new Object();
    for (var i = 0 ; i < controls.length; i++) {
        var control = controls[i];
        if ($.trim(control.name) != "") {
            resultMap[control.name] = control.value;
        }
    }
    return JSON.stringify(resultMap);
}

var visitServer = 0;
function submitData(url, data, callback) {
    if (visitServer == 1) {
        return;
    }
    visitServer = 1;
    $.ajax({
        type: "post",
        url: url,
        data: data,
        beforeSend: loadStart(),
        success: function (data) {
            visitServer = 0;
            loadEnd();

            filterData(data, callback);
            //var callData = 
            //if (callback != null && callback != undefined)
            //{
            //    callback(callData);
            //}
        },
        error: function (ex) {
            alert(JSON.stringify(ex))
            loadEnd();
            layer.alert('提交数据的时候，出现未知错误!', { icon: 2, skin: 'layer-ext-moon' });
        }
    });
}
var common_load_index;
function loadStart(msg) {
    common_load_index = layer.load(1,
    {
        shade: [0.1, '#000']
    });
}
function loadEnd() {
    layer.close(common_load_index);
}

//过滤返回值
function filterData(text, callback) {
    var pindex = null;

    if (parent.layer != null && parent.layer != undefined) {
        pindex = parent.layer.getFrameIndex(window.name);
    }

    if (text == null || text == "") {
        layer.alert('操作已完成!', { icon: 1, skin: 'layer-ext-moon' });
        if (callback != null && callback != undefined) {
            callback([]);
            return;
        }
    }

    try {
        data = eval('(' + text + ')');

        if (data.ResultType == null || data.ResultType == undefined || data.ResultType == "") {
            layer.alert("非法数据！", { icon: 2, skin: 'layer-ext-moon' });
            return "[]";
        }
        switch (data.ResultType) {
            case 1:
                if (callback != null && callback != undefined) {
                    callback(eval('(' + data.ResultData + ')'));
                    return;
                }
                break;
            case 2:
                layer.alert(data.ResultData, { icon: 1, skin: 'layer-ext-moon' }, function (index) {

                    layer.close(index);
                    if (callback != null && callback != undefined) {
                        callback([]);
                        return;
                    }
                });
                break;
            case 3:
                layer.alert(data.ResultData, { icon: 1, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);
                    
                    parent.hbesWinCloseType = "ok";
                   
                    parent.layer.close(pindex);

                });

                return "[]";
                break;
            case 4:
                layer.alert(data.ResultData, { icon: 0, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);

                    if (callback != null && callback != undefined) {
                        callback([]);
                        return;
                    }
                });
                break;
            case 5:
                layer.alert(data.ResultData, { icon: 0, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);
                    parent.hbesWinCloseType = "ok";
                    parent.layer.close(pindex);

                });
            case 6:
                layer.alert(data.ResultData, { icon: 0, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);

                    if (callback != null && callback != undefined) {
                        callback([]);
                        return;
                    }
                });
                break;
            case 7:
                layer.alert(data.ResultData, { icon: 0, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);
                    parent.hbesWinCloseType = "ok";
                    parent.layer.close(pindex);
                });
                return "[]";
                break;
            case 8:
                layer.alert("登录过期请重新登录！", { icon: 0, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);
                    window.top.location.href = "/Login.aspx";
                });
                return "[]";
                break;
            case 9: layer.alert(data.ResultData, { icon: 2, skin: 'layer-ext-moon' });
                return "[]";
                break;
        }

    }
    catch (e) {

        layer.alert("数据处理错误！", { icon: 2, skin: 'layer-ext-moon' });
    }
}

function handlerStr(text) {
    if (text == null || text == undefined) {
        return "";
    }
    return text;
}

$(function () {

    $(".page-grid").gridInit();

});

var allGrid = new Array();

var SelectAllWorkers_valuesData = "";
var SelectAllWorkers_lx = "";
function createGrid(gridEl) {

    var obj = new Object();
    obj = {
        el: $(gridEl),
        data: [],
        url: "",
        getUrl: function () {
            return obj.getUrl;
        },
        setUrl: function (url) {
            obj.url = url;
        },
        getData: function () {

            return obj.data;
        },
        setData: function (data) {
            if (obj.showpager + "" == "true") {
                if (data != null && data != undefined) {
                    if (data.data != null && data.data != undefined) {
                        obj.data = data.data;
                    }
                    else {
                        obj.data = [];
                    }
                    if (data.total != null && data.total != undefined) {
                        obj.setTotal(data.total);
                    }
                    else {
                        obj.setTotal(0);
                    }
                }
                else {
                    obj.data = [];
                    obj.setTotal(0);
                }
            }
            else {
                obj.data = data;
            }
        },
        pagelist: [10, 25, 50, 100],
        getPagelist: function () {
            return obj.pagelist;
        },
        setPageList: function (pagelist) {
            obj.pagelist = pagelist;
            obj.pagesize = obj.pagelist[0];
        },
        pagesize: 10,
        getPagesize: function () {

            return obj.pagesize;
        },
        setPagesize: function (pagesize) {
            obj.pagesize = pagesize;
        },
        pageindex: 1,
        getPageindex: function () {
            return obj.pageindex;
        },
        setPageindex: function (pageindex) {
            obj.pageindex = pageindex;
            if (obj.pageindex > obj.countpage) {
                obj.pageindex = obj.countpage;
            }
            if (obj.pageindex < 1) {
                obj.pageindex = 1;
            }
        },
        total: 0,
        getTotal: function () {
            return obj.total;
        },
        setTotal: function (total) {
            obj.total = total;
            obj.countpage = Math.ceil(obj.total / obj.pagesize);
        },
        countpage: 1,
        getCountpage: function () {
            return obj.countpage;
        },

        showpager: true,
        columns: [],
        filterData: {},
        getFilterData: function () {
            return obj.filterData;
        },
        setFilterData: function (data) {
            return obj.filterData = data;
        },
        multiSelect: false,
        showpagealige: "up",
        selectRows_: new Object(),
        selectClick: null,
        checkClick: null,
        headCheckClick: null,
        dbrowclick: null,

    };

    var selectClick = $(gridEl).attr("selectClick");

    if (selectClick != undefined) {
        obj.selectClick = selectClick;
    }

    var checkClick = $(gridEl).attr("checkClick");
    if (checkClick != undefined) {
        obj.checkClick = checkClick;
    }


    var headCheckClick = $(gridEl).attr("headCheckClick");
    if (headCheckClick != undefined) {
        obj.headCheckClick = headCheckClick;
    }

    obj.selectRows_["000"] = true;

    var showpager = $(gridEl).attr("showpager");

    if (showpager != undefined) {
        obj.showpager = showpager;
    }

    var url = $(gridEl).attr("url");

    if (url != undefined) {
        obj.url = url;
    }

    var showpagealige = $(gridEl).attr("showpagealige");
    if (showpagealige != undefined) {

        obj.showpagealige = showpagealige;
    }


    var pagelist = $(gridEl).attr("pagelist");
    if (pagelist != undefined) {

        obj.setPageList(eval(pagelist));
    }
    var data = $(gridEl).attr("data");
    if (data != undefined) {
        obj.setData(eval(data));
        if (obj.showpager) {
            obj.setTotal(data.total);
        }
    }
    var pagesize = $(gridEl).attr("pagesize");
    if (pagesize != undefined) {
        obj.setPagesize(pagesize);
    }
    var pageindex = $(gridEl).attr("pageindex");
    if (pageindex != undefined) {
        obj.setPageindex(pageindex);
    }

    var multiSelect = $(gridEl).attr("multiSelect");
    if (multiSelect != undefined) {
        obj.multiSelect = multiSelect;
    }

    var dbrowclick = $(gridEl).attr("dbrowclick");
    if (dbrowclick != undefined) {
        obj.dbrowclick = dbrowclick;
    }


    var columnsEl = $(gridEl).children();
    columnsEl.hide();
    var columnElArray = $(columnsEl).children();

    for (var i = 0 ; i < columnElArray.length ; i++) {
        var columnEl = columnElArray[i];
        var column = new Object();
        if ($(columnEl).attr("field") != undefined) {
            column.field = $(columnEl).attr("field");
        }
        else {
            column.field = "";
        }

        if ($.trim($(columnEl).html()) != "") {
            column.name = $(columnEl).html();
        }
        else {
            column.name = "";
        }
        if ($(columnEl).attr("width") != undefined) {
            column.width = $(columnEl).attr("width");
        }
        else {
            column.width = "";
        }
        //indexcolumn为数字列、checkcolumn为复选框列

        if ($(columnEl).attr("type") != undefined) {
            column.type = $(columnEl).attr("type");
        }
        else {
            column.type = "datacolumn";
        }
        if ($(columnEl).attr("field") != undefined) {
            column.field = $(columnEl).attr("field");
        }

        if ($(columnEl).attr("headAlign") != undefined) {
            column.headAlign = $(columnEl).attr("headAlign");
        }
        else {
            column.headAlign = "center";
        }

        if ($(columnEl).attr("textAlign") != undefined) {
            column.textAlign = $(columnEl).attr("textAlign");
        }
        else {
            column.textAlign = "center";
        }

        if ($(columnEl).attr("load") != undefined) {
            column.load = $(columnEl).attr("load");
        }
        else {
            column.load = "";
        }

        obj.columns.push(column);

    }
    allGrid.push(obj);
};
var selectTrOrCk = 0;
var hbes =
{
    get: function (elId) { return $("#" + elId); },
    //下一页
    nextPage: function (index) {

        var grid = allGrid[index];

        if (grid.getPageindex() < grid.getCountpage()) {

            grid.setPageindex(grid.getPageindex() + 1);
            $(grid.el).reload();
        }
        else {

        }


    },
    //上一页
    prevPage: function (index) {
        var grid = allGrid[index];
        if (grid.getPageindex() > 1) {

            grid.setPageindex(grid.getPageindex() - 1);
            $(grid.el).reload();
        }

    },
    //首页
    firstPage: function (index) {
        var grid = allGrid[index];
        if (grid.getPageindex() != 1) {
            grid.setPageindex(1);
            $(grid.el).reload();
        }
    },
    //尾页
    lastPage: function (index) {

        var grid = allGrid[index];
        if (grid.getCountpage() != grid.getPageindex()) {
            grid.setPageindex(grid.getCountpage());
            $(grid.el).reload();
        }
    },
    //选择每页大小
    selectPagesize: function (index, value) {
        var grid = allGrid[index];
        grid.setPagesize(value);
        $(grid.el).loadGridData();
    },
    //输入当前页
    inPage: function (index, el) {
        var grid = allGrid[index];
        var pageIndex = $.trim(el.value);
        var test_ = /^[0-9]*[1-9][0-9]*$/;
        if (test_.test(pageIndex)) {
            grid.setPageindex(pageIndex);
        }
        else {
            grid.setPageindex(1);
        }
        $(grid.el).reload();
    },

    selectRow: function (gridIndex, rowIndex) {//#F5F5F5
        // $(".tr" + gridIndex).css("background-color", "red");
        var grid = allGrid[gridIndex];
        if (selectTrOrCk == 1) {
            selectTrOrCk = 0;
            if (grid.checkClick != null && grid.checkClick != undefined && grid.checkClick != "") {
                var func = eval(grid.checkClick);
                new func(grid.getData()[rowIndex]);
            }
            return;
        }
        hbes.cancelSelect(gridIndex);
        grid.selectRows_[rowIndex] = true;
        if (document.getElementById("ck" + gridIndex + rowIndex) != null && document.getElementById("ck" + gridIndex + rowIndex) != undefined) {
            document.getElementById("ck" + gridIndex + rowIndex).checked = true;
        }

        $("#tr" + gridIndex + rowIndex).css("background-color", "#9AADB5");
        if (grid.selectClick != null && grid.selectClick != undefined && grid.selectClick != "") {
            var func = eval(grid.selectClick);
            new func(grid.getData()[rowIndex]);
        }

    },
    selecCk: function (gridIndex, rowIndex) {
        var grid = allGrid[gridIndex];
        var isChecked = document.getElementById("ck" + gridIndex + rowIndex).checked;
        selectTrOrCk = 1;
        if (isChecked) {

            $("#tr" + gridIndex + rowIndex).css("background-color", "#9AADB5");
            grid.selectRows_[rowIndex] = true;
        }
        else {
            $("#tr" + gridIndex + rowIndex).css("background-color", "");
            delete grid.selectRows_[rowIndex];
        }

    },
    rowdbclick: function (gridIndex, rowIndex) {
        var grid = allGrid[gridIndex];
        if (grid.dbrowclick != null && grid.dbrowclick != undefined && grid.dbrowclick != "") {
            var row = (grid.getData()[rowIndex]);
            var func = eval(grid.dbrowclick);
            new func(row);
        }
    }
    ,
    cancelSelect: function (gridIndex) {
        var grid = allGrid[gridIndex];
        $(".tr" + gridIndex).css("background-color", "");
        var ck = $(".ck" + gridIndex);
        for (var i = 0 ; i < ck.length; i++) {
            ck[i].checked = false;
        }

        for (var key in grid.selectRows_) {
            delete grid.selectRows_[key];
        }
    },
    okSelect: function (gridIndex) {
        var grid = allGrid[gridIndex];

        $(".tr" + gridIndex).css("background-color", "#9AADB5");
        var ck = $(".ck" + gridIndex);
        for (var i = 0 ; i < ck.length; i++) {
            ck[i].checked = true;
            grid.selectRows_[i] = true;
        }
    },
    selectCkAll: function (gridIndex, isChecked) {

        var grid = allGrid[gridIndex];


        if (isChecked) {

            hbes.okSelect(gridIndex);
        }
        else {
            hbes.cancelSelect(gridIndex);
        }
        if (grid.headCheckClick != null && grid.headCheckClick != undefined && grid.headCheckClick != "") {
            var func = eval(grid.headCheckClick);
            new func();
        }
    },
    //加载 数据列表 是，根据SetValuesData，选中 复选框 ,SetValuesData 是 数组
    setHebsGrideSetValues: function (gridIndex, SetValuesData, lx) {
        /*
        gridIndex:grid 编号
        SetValuesData：默认值 数组
        lx：类型 自己写的死值
        */
        parent.SelectAllWorkers_valuesData = SetValuesData.split(",");
        parent.SelectAllWorkers_lx = lx;
    }
};

(function ($) {
    $.fn.extend({
        //初始化
        gridInit: function () {

            var gridArray = this;

            if (gridArray.length > 0) {
                for (var i = 0 ; i < gridArray.length; i++) {

                    var gridEl = gridArray[i];

                    if (gridEl != null && gridEl != undefined && gridEl != "") {
                        createGrid(gridEl);

                    }

                }
            }

        },
        //获取数据
        loadGridData: function (filterData, onSuccess) {
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {

                    var grid = allGrid[i];

                    if (this.length <= 1) {
                        if (!$(this).is(grid.el)) {
                            continue;
                        }
                    }
                    hbes.cancelSelect(i);
                    grid.setPageindex(1);
                    if ($.trim(grid.url) != "") {
                        if (filterData != null && filterData != undefined && filterData != "") {
                            grid.setFilterData(filterData);
                        }

                        grid.filterData.pageIndex = grid.getPageindex();
                        grid.filterData.pageSize = grid.getPagesize();

                        $.ajax({
                            type: "POST",
                            url: $.trim(grid.url),
                            data: grid.getFilterData(),
                            beforeSend: ajaxLoading(grid.el),
                            success: function (data) {

                                ajaxLoadEnd();
                                var data_ = filterData_(data);

                                grid.setData(data_);
                                if (grid.showpagealige == "up") {
                                    $(grid.el).splitPager();
                                    $(grid.el).loadGridHtml();
                                }
                                else if (grid.showpagealige == "bottom") {
                                    $(grid.el).loadGridHtml();
                                    $(grid.el).splitPager();

                                }
                                else {
                                    $(grid.el).splitPager();
                                    $(grid.el).loadGridHtml();
                                }

                                if (onSuccess != null && onSuccess != undefined) {
                                    onSuccess();
                                }
                            },
                            error: function (e) {
                                alert("error");
                            }
                        });
                    }
                    else {
                        if (grid.showpagealige == "up") {
                            $(grid.el).splitPager();
                            $(grid.el).loadGridHtml();
                        }
                        else if (grid.showpagealige == "bottom") {
                            $(grid.el).loadGridHtml();
                            $(grid.el).splitPager();

                        }
                        else {
                            $(grid.el).splitPager();
                            $(grid.el).loadGridHtml();
                        }
                    }

                }
            }
        },
        reload: function (onSuccess) {
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {

                    var grid = allGrid[i];
                    if (this.length <= 1) {


                        if (!$(this).is(grid.el)) {
                            continue;
                        };
                    }
                    hbes.cancelSelect(i);
                    if ($.trim(grid.url) != "") {


                        grid.filterData.pageIndex = grid.getPageindex();
                        grid.filterData.pageSize = grid.getPagesize();

                        $.ajax({
                            type: "POST",
                            url: $.trim(grid.url),
                            data: grid.getFilterData(),
                            beforeSend: ajaxLoading(grid.el),
                            success: function (data) {
                                ajaxLoadEnd();
                                var data_ = filterData_(data);
                                grid.setData(data_);
                                if (grid.showpagealige == "up") {
                                    $(grid.el).splitPager();
                                    $(grid.el).loadGridHtml();
                                }
                                else if (grid.showpagealige == "bottom") {
                                    $(grid.el).loadGridHtml();
                                    $(grid.el).splitPager();

                                }
                                else {
                                    $(grid.el).splitPager();
                                    $(grid.el).loadGridHtml();
                                }
                                if (onSuccess != null && onSuccess != undefined) {
                                    onSuccess();
                                }
                            },
                            error: function (e) {
                                alert("error");
                            }
                        });
                    }
                    else {
                        if (grid.showpagealige == "up") {
                            $(grid.el).splitPager();
                            $(grid.el).loadGridHtml();
                        }
                        else if (grid.showpagealige == "bottom") {
                            $(grid.el).loadGridHtml();
                            $(grid.el).splitPager();

                        }
                        else {
                            $(grid.el).splitPager();
                            $(grid.el).loadGridHtml();
                        }
                    }

                }
            }
        },
        //分页
        splitPager: function () {
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if (!$(this).is(grid.el)) {
                        continue;
                    }
                    if ((grid.showpager + "") == "true") {
                        var pagesplitHtml = "<div id=\"page-grid-size" + i + "\"><div class=\"page-grid-size\">";
                        pagesplitHtml += "<select onchange=\"hbes.selectPagesize(" + i + ",this.value)\">";
                        for (var j = 0 ; j < grid.pagelist.length ; j++) {
                            var pagesize = grid.pagesize;
                            if (grid.pagelist[j] != pagesize) {
                                pagesplitHtml += " <option value=\"" + grid.pagelist[j] + "\">" + grid.pagelist[j] + "</option>";
                            }
                            else {
                                pagesplitHtml += " <option value=\"" + grid.pagelist[j] + "\" selected=\"selected\">" + grid.pagelist[j] + "</option>";
                            }
                        }
                        pagesplitHtml += " </select>";
                        pagesplitHtml += "<a href=\"javascript:hbes.firstPage(" + i + ");\" class=\" button\">首页</a>";
                        pagesplitHtml += "<a href=\"javascript:hbes.prevPage(" + i + ");\"   class=\"button\">上一页</a>";
                        pagesplitHtml += " <span>第";
                        pagesplitHtml += "<input type=\"text\" onchange=\"hbes.inPage(" + i + ",this)\" value=\"" + grid.getPageindex() + "\" />共" + grid.getCountpage() + "页</span>";
                        pagesplitHtml += " <a href=\"javascript:hbes.nextPage(" + i + ");\" class=\"button\">下一页</a>";
                        pagesplitHtml += "<a href=\"javascript:hbes.lastPage(" + i + ");\" class=\"button\">尾页</a></div>";
                        pagesplitHtml += "<div class=\"page-grid-bars\">";

                        pagesplitHtml += " <span>显示" + ((grid.getPageindex() - 1) * grid.getPagesize() + 1) + "到" + ((grid.getPageindex() - 1) * grid.getPagesize() + grid.getData().length) + "，共" + grid.getTotal() + "条记录</span></div></div>";
                        $("#page-grid-size" + i).remove();
                        $(grid.el).append(pagesplitHtml);
                    }
                }
            }
        },
        //展示数据
        loadGridHtml: function () {
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if (!$(this).is(grid.el)) {
                        continue;
                    }
                    var columnsEl = $(grid.el).children();

                    for (var j = 0 ; j < columnsEl.length; j++) {
                        if ($(columnsEl[j]).attr("property") == "columns") {
                            $(columnsEl[j]).remove();
                        }
                    }
                    var data = new Object();

                    data = grid.getData();
                    //展示头部
                    var html = " <div id=\"page-grid-div" + i + "\"><table style='table-layout:fixed ;' id=\"page-grid-table" + i + "\" class=\"table table-border table-bordered table-bg mytable-hover table-sort dataTable no-footer\"  role=\"grid\" aria-describedby=\"DataTables_Table_0_info\">";
                    html += " <thead>";
                    html += " <tr class=\"text-c\" role=\"row\">";

                    for (var j = 0 ; j < grid.columns.length; j++) {
                        var column = grid.columns[j];
                        var width = "";
                        if ($.trim(column.width) != "") {
                            width = "width:" + $.trim(column.width) + "px;";
                        }

                        if (column.type == "indexcolumn") {
                            html += " <th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"" + width + " text-align:" + column.headAlign + "  \"></th>";
                        }
                        else if (column.type == "checkcolumn") {
                            html += " <th class=\"sorting_disabled\"  rowspan=\"1\" colspan=\"1\" style=\"" + width + " text-align:" + column.headAlign + "  \">";
                            html += "<input onclick=\"hbes.selectCkAll(" + i + ",this.checked)\" name=\"\" value=\"\" type=\"checkbox\" /></th>";
                        }
                        else {
                            html += " <th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"" + width + "  text-align:" + column.headAlign + " \">";

                            html += column.name + "</th>";
                        }
                    }
                    html += "</tr></thead>";
                    //展示内容
                    html += "<tbody>";
                    var classD = "odd";

                    for (var j = 0 ; j < data.length; j++) {

                        html += "<tr ondblclick='hbes.rowdbclick(" + i + "," + j + ")' onclick=\"hbes.selectRow(" + i + "," + j + ")\"  id=\"tr" + i + "" + j + "\"  class=\"text-c " + classD + " tr" + i + "" + "\" role=\"row\">";
                        if (classD == "odd") {
                            classD = "even";
                        }
                        else {
                            classD = "odd";
                        }
                      
                        for (var c = 0 ; c < grid.columns.length; c++) {
                            var column = grid.columns[c];
                            if (column.type == "indexcolumn") {
                                html += "<td style=\"text-align:" + column.textAlign + "\">" + (j + 1) + "</td>";
                            }
                            else if (column.type == "checkcolumn") {
                                //jxg add 2017-02-21
                                //var SelectAllWorkers_valuesData = SetValuesData.split(",");
                                //var SelectAllWorkers_lx = lx;   UnitsGuid
                                var cks = "";
                                html += "<td style=\"text-align:" + column.textAlign + "\"> <input value=\"1\" onclick=\"hbes.selecCk(" + i + "," + j + ")\" id=\"ck" + i + "" + j + "\" class=\"ck" + i + "\" type=\"checkbox\" " + cks + " /></td>";

                            }
                            else {
                                var text = data[j][column.field];
                                if (text == null || text == undefined) {
                                    text = "";
                                }
                                var title = text;
                                if (column.load != null && column.load != undefined && column.load != "")
                                {
                                    var func = eval(column.load);
                                    text = func(data[j]);
                                    title = ""
                                }

                                html += "<td title='" + title + "' style=\"text-align:" + column.textAlign + ";\" class='texttd'><p class='ut-s'>" + text + "</p></td>";
                            }
                        }
                        html += "</tr>";
                    }
                    html += "</tbody></table></div>";
                    $("#page-grid-div" + i).remove();
                    $(grid.el).append(html);
                }
            }
        },
        getHbesGridSelected: function () {
            var index = 0;
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if ($(this).is(grid.el)) {
                        index = i;
                        break;
                    }
                }

                var grid = allGrid[index];

                if (grid != null && grid != undefined) {

                    for (var key in grid.selectRows_) {

                        if (key == "000") {
                            return null;
                        }
                        return grid.getData()[key];
                    }
                    return null;
                }
                else {
                    return null;
                }
            }
        },
        getHbesGridSelecteds: function () {
            var index = 0;
            var resultArray = new Array();

            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];

                    if ($(this).is(grid.el)) {
                        index = i;
                        break;
                    }
                }

                var grid = allGrid[index];

                if (grid != null && grid != undefined) {
                    for (var key in grid.selectRows_) {

                        if (key != "000") {
                            resultArray.push(grid.getData()[key]);
                        }
                    }
                    if (resultArray.length == 0) {
                        return null;
                    }
                    return resultArray;
                }
                else {
                    return null;
                }
            }

        }
    });
})(jQuery);

$(function ()
{

    $(".hbes-tree").treeInit();

});

var allTree = new Array();
function isHideTree(inputEl, treeId)
{
    var x = window.event.clientX;
    var y = window.event.clientY;
    var tree = document.getElementById(treeId);
    var divx1 = tree.offsetLeft;
    var divy1 = tree.offsetTop;
    var divx2 = tree.offsetLeft + tree.offsetWidth;
    var divy2 = tree.offsetTop + tree.offsetHeight;
}

function createTree(treeEl) {
    var obj = new Object();
    obj = {
        el: $(treeEl),
        url: "",
        data: [],
        idFied: "id",
        pidFied: "pid",
        textFied: 'text',
        treeClick: null,
        loadSuccess: null,
        checkedEnable: false,//多选
        chkStyle: "checkbox",//ztree 默认复选框
        onCheckCallback: null        //beforeCheck / onCheck 选择复选框的 回调函数
    };

    var url = $(treeEl).attr("url");
    if (url != undefined) {
        obj.url = url;
    }

    var data = $(treeEl).attr("data");
    if (data != undefined) {
        obj.data = eval(data);
    }

    var idFied = $(treeEl).attr("idFied");
    if (idFied != undefined) {
        obj.idFied = idFied;
    }

    var pidFied = $(treeEl).attr("pidFied");
    if (pidFied != undefined) {
        obj.pidFied = pidFied;
    }

    var textFied = $(treeEl).attr("textFied");
    if (textFied != undefined) {
        obj.textFied = textFied;
    }

    var treeClick = $(treeEl).attr("treeClick");
    if (treeClick != undefined) {
        obj.treeClick = treeClick;
    }
    var loadSuccess = $(treeEl).attr("loadSuccess");
    if (loadSuccess != undefined) {
        obj.loadSuccess = loadSuccess;
    }

    var checkedEnable = $(treeEl).attr("checkedEnable");//是否启用 
    if (checkedEnable != undefined) {
        obj.checkedEnable = checkedEnable;
    }

    var chkStyle = $(treeEl).attr("chkStyle");//显示 单选、复选框
    if (chkStyle != undefined) {
        obj.chkStyle = chkStyle;
    }

    var onCheckCallback = $(treeEl).attr("onCheckCallback");//选择复选框的回调
    if (onCheckCallback != undefined) {
        obj.onCheckCallback = onCheckCallback;
    }

    allTree.push(obj);
}

function submitTreeData(url, data, callback) {

    $.ajax({
        type: "post",
        url: url,
        data: data,
        async: false,
        beforeSend: loadStart(),
        success: function (data) {
            loadEnd();

            filterData(data, callback);
        },
        error: function (ex) {
            loadEnd();
            layer.alert('提交数据的时候，出现未知错误!', { icon: 2, skin: 'layer-ext-moon' });
        }
    });
}
(function ($) {
    $.fn.extend({
        treeInit: function () {
            var treeArray = this;

            if (treeArray.length > 0) {
                for (var i = 0 ; i < treeArray.length; i++) {

                    var treeEl = treeArray[i];

                    if (treeEl != null && treeEl != undefined && treeEl != "") {
                        createTree(treeEl);
                        $(treeEl).treeBind();
                    }

                }
            }

        },
        treeBind: function (formData) {

            var treeIndex = null;

            if (allTree.length > 0) {
                for (var i = 0 ; i < allTree.length; i++) {
                    var treeOne = allTree[i];
                    if ($(this).is(treeOne.el)) {
                        treeIndex = i;
                        break;
                    }
                }
            }


            if (treeIndex == null) {
                return;
            }
            var tree = allTree[treeIndex];


            if ($.trim(tree.url) != "") {
                var data_ = {};
                if (formData != null && formData != undefined) {
                    data_ = formData;
                }
                submitTreeData(tree.url, data_, function (data) {

                    allTree[treeIndex].data = data;

                    $(allTree[treeIndex].el).treeShow();
                });
            }
            else {
                $($(allTree[treeIndex].el)).treeShow();
            };
        },
        treeShow: function () {

            var treeIndex = null;

            if (allTree.length > 0) {
                for (var i = 0 ; i < allTree.length; i++) {
                    var treeOne = allTree[i];

                    if ($(this).is(treeOne.el)) {
                        treeIndex = i;
                        break;
                    }
                }
            }

            if (treeIndex == null) {
                return;
            }
            var tree = allTree[treeIndex];


            var setting = {
                check: {
                    enable: tree.checkedEnable == "true" ? true : false,
                    chkStyle: tree.chkStyle,
                    autoCheckTrigger: true
                },
                data: {
                    key: {
                        name: tree.textFied
                    },
                    simpleData: {
                        enable: true,
                        idKey: tree.idFied,//节点绑定的字段(数据库表字段groupid)
                        pIdKey: tree.pidFied//节点的上级字段(数据库表字段parentid)
                    }
                },
                callback: {
                    onClick: function (event, treeId, treeNode) {
                        if (tree.treeClick != null && tree.treeClick != undefined && tree.treeClick != "") {
                            var func = eval(tree.treeClick);
                            new func(treeNode);
                        }
                    },
                    onCheck: function (event, treeId, treeNode) {
                        if (tree.onCheckCallback != null && tree.onCheckCallback != undefined && tree.onCheckCallback != "") {
                            var func = eval(tree.onCheckCallback);
                            new func(treeNode);
                        }
                    }
                }
            };

            var myTree = $.fn.zTree.init(tree.el, setting, tree.data);
            allTree[treeIndex].tree = myTree;

            if (tree.loadSuccess != null && tree.loadSuccess != undefined && tree.loadSuccess != "") {

                var func = eval(tree.loadSuccess);
                new func(myTree);
            }

        },
        getTree: function () {
            var treeIndex = null;

            if (allTree.length > 0) {
                for (var i = 0 ; i < allTree.length; i++) {
                    var treeOne = allTree[i];

                    if ($(this).is(treeOne.el)) {
                        treeIndex = i;
                        break;
                    }
                }
            }

            if (treeIndex == null) {
                return null;
            }
            return allTree[treeIndex].tree;
        },
        LoadTreeTwo: function (dataArry, treeid, DWsetting) {
            if (dataArry && dataArry != undefined) {
                var ztreeObj = $.fn.zTree.init($("#" + treeid), DWsetting, dataArry);
            }
        }
    });
})(jQuery);