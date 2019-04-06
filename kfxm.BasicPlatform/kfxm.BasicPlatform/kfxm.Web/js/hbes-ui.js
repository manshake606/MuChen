
var relationshipWinName = "";
var hbesWinCloseType = "error";
$(function () {
    submitData("/Home/GetButton", {}, function (data) {
        if (data != null && data != undefined && data.length > 0) {
            for (var i = 0 ; i < data.length ; i++) {
                var buttonId = data[i];
                $("#" + buttonId).css("display", "inline");
            }
        }
    });
});
document.onkeydown = function () {
    if (event.keyCode == 8) {// 获取点击返回键的编号  
        if (event.srcElement.tagName.toLowerCase() != "input"
                     && event.srcElement.tagName.toLowerCase() != "textarea"
                      && event.srcElement.tagName.toLowerCase() != "password")// 判断当前的焦点是否在input、textarea、password节点里  
            event.returnValue = false;
        if (event.srcElement.readOnly == true
                 || event.srcElement.disabled == true)// 判断当前的焦点是否在一个readonly、disabled节点里  
            event.returnValue = false;
    }

}
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
        shadeClose: false,
        maxmin: true,
        closeBtn: 1,
        content: url,
        scrollbar: 'true',
        yes: function (index, layero) {
            alert(index);
            var method = "save";
            if (okRunMethodName != null && okRunMethodName != undefined && $.trim(okRunMethodName) != "") {
                method = okRunMethodName;
            }
            //var body = layer.getChildFrame('body', index);
        },
        end: function () {
            if (parent.hbesWinCloseType == "ok") {
                parent.hbesWinCloseType = "error";
                if (okRunMethodName != null && okRunMethodName != undefined && $.trim(okRunMethodName) != "") {
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

    var controls = $("#" + id + " .hbes-input");
    var resultMap = new Object();
    for (var i = 0 ; i < controls.length; i++) {
        var control = controls[i];
        if ($.trim(control.name) != "") {
            resultMap[control.name] = control.value;
        }
    }
    var controls = $("#" + id + " .layui-input");
    for (var i = 0 ; i < controls.length; i++) {
        var control = controls[i];
        if ($.trim(control.name) != "") {
            resultMap[control.name] = control.value;
        }
    }

    var comboxs = $("#" + id + " .hbes-combox");
    for (var i = 0 ; i < comboxs.length; i++) {
        var combox = comboxs[i];
        if ($.trim(combox.name) != "") {
            resultMap[combox.name] = $(combox).comboxGetValue();
        }
    }
    var radios = $("#" + id + " :radio:checked");
    if (radios.length > 0) {
        for (var i = 0; i < radios.length; i++) {
            var _radio = radios[i];
            var radioName = radios[i].name;
            resultMap[radioName] = _radio.value;
        }
    }
    
    var select = $(".layui-select");
    var _select = select.val();
    var selectName = select.attr("name");
    resultMap[selectName] = _select;
    
        
    
    return JSON.stringify(resultMap);
}
function getLayUIFormMap(id) {
    var controls = $("#" + id + " .layui-input");
    var resultMap = new Object();
    for (var i = 0 ; i < controls.length; i++) {
        var control = controls[i];
        if ($.trim(control.name) != "") {
            resultMap[control.name] = control.value;
        }
    }
    //var comboxs = $("#" + id + " .hbes-combox");
    //for (var i = 0 ; i < comboxs.length; i++) {
    //    var combox = comboxs[i];
    //    if ($.trim(combox.name) != "") {
    //        resultMap[combox.name] = $(combox).comboxGetValue();
    //    }
    //}
    return JSON.stringify(resultMap);
}

function submitData(url, data, callback) {
    var IsHandler = "IsHandler=1";
    if (url.indexOf("?") > -1) {
        url = url + "&" + IsHandler;
    }
    else {
        url = url + "?" + IsHandler;
    }
    loadStart();
    $.ajax({
        type: "post",
        url: url,
        data: data,
        beforeSend: loadStart(),
        success: function (data) {

            loadEnd();
            ResultFilterData(data, callback);
        },
        error: function (ex) {
            loadEnd();
            layer.alert('提交数据的时候，出现未知错误!', { icon: 2, skin: 'layer-ext-moon' });
        }
    });
}

function submitDataNoResultFilter(url, data, callback) {
    var IsHandler = "IsHandler=1";
    if (url.indexOf("?") > -1) {
        url = url + "&" + IsHandler;
    }
    else {
        url = url + "?" + IsHandler;
    }
    loadStart();
    $.ajax({
        type: "post",
        url: url,
        data: data,
        beforeSend: loadStart(),
        success: function (data) {

            loadEnd();
            if (callback != null && callback != undefined) {
                callback([]);
            }
        },
        error: function (ex) {
            loadEnd();
            layer.alert('提交数据的时候，出现未知错误!', { icon: 2, skin: 'layer-ext-moon' });
        }
    });
}


function submitDataNoLoad(url, data, callback) {
    var IsHandler = "IsHandler=1";
    if (url.indexOf("?") > -1) {
        url = url + "&" + IsHandler;
    }
    else {
        url = url + "?" + IsHandler;
    }
    loadStart();
    $.ajax({
        type: "post",
        url: url,
        data: data,
        success: function (data) {
            ResultFilterData(data, callback);
        },
        error: function (ex) {

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
function ResultFilterData(text, callback) {

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

        var data = eval('(' + text + ')');

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
                    if (callback != null && callback != undefined) {
                        callback([]);
                    }
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
                layer.alert(data.ResultData, { icon: 2, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);

                    if (callback != null && callback != undefined) {
                        callback([]);
                        return;
                    }
                });
                break;
            case 7:
                layer.alert(data.ResultData, { icon: 2, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);
                    parent.hbesWinCloseType = "ok";
                    parent.layer.close(pindex);
                });
                return "[]";
                break;
            case 8:
                layer.alert("登录过期请重新登录！", { icon: 0, skin: 'layer-ext-moon' }, function (index) {
                    layer.close(index);
                    window.top.location.href = "/Login";
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
        pagelist: [10, 20, 50, 100],
        getPagelist: function () {
            return obj.pagelist;
        },
        setPageList: function (pagelist) {
            obj.pagelist = pagelist;
            obj.pagesize = obj.pagelist[0];
        },
        pagesize: 20,
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
        headLoad: null,
        rowLoad: null,
        onselectionchanged: null,        //增加onselectionchanged事件 20170620 by ylb
    };

    var selectClick = $(gridEl).attr("selectClick");
    if (selectClick != undefined) {
        obj.selectClick = selectClick;
    }

    var checkClick = $(gridEl).attr("checkClick");
    if (checkClick != undefined) {
        obj.checkClick = checkClick;
    }

    var onselectionchanged = $(gridEl).attr("onselectionchanged");
    if (onselectionchanged != undefined) {
        obj.onselectionchanged = onselectionchanged;
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

    var headLoad = $(gridEl).attr("headLoad");
    if (headLoad != undefined) {
        obj.headLoad = headLoad;
    }

    var rowLoad = $(gridEl).attr("rowLoad");
    if (rowLoad != undefined) {
        obj.rowLoad = rowLoad;
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

    //// 增加disable(id) 20170620 by ylb
    disable: function (elId) { $("#" + elId).attr("disabled", true); $("#" + elId).addClass("disabled"); },

    //// 增加enable(id) 20170620 by ylb
    enable: function (elId) { $("#" + elId).attr("disabled", false); $("#" + elId).removeClass("disabled"); },

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
    //选择一行
    selectRow: function (gridIndex, rowIndex) {//#F5F5F5
        // $(".tr" + gridIndex).css("background-color", "red");
        var grid = allGrid[gridIndex];
        if (selectTrOrCk == 1) {
            selectTrOrCk = 0;
            if (grid.checkClick != null && grid.checkClick != undefined && grid.checkClick != "") {
                var func = eval(grid.checkClick);
                new func(grid.getData()[rowIndex]);
            }
            if (grid.onselectionchanged != null && grid.onselectionchanged != undefined && grid.onselectionchanged != "") {
                var func = eval(grid.onselectionchanged);
                new func(grid.getData()[rowIndex]);
            }
            return;
        }
        hbes.cancelSelect(gridIndex);
        grid.selectRows_[rowIndex] = true;
        if (document.getElementById("ck" + gridIndex + rowIndex) != null && document.getElementById("ck" + gridIndex + rowIndex) != undefined) {
            document.getElementById("ck" + gridIndex + rowIndex).checked = true;
        }

        $("#tr" + gridIndex + rowIndex).css("background-color", "#e2e2e2");
        if (grid.selectClick != null && grid.selectClick != undefined && grid.selectClick != "") {
            var func = eval(grid.selectClick);
            new func(grid.getData()[rowIndex]);
        }
        if (grid.onselectionchanged != null && grid.onselectionchanged != undefined && grid.onselectionchanged != "") {
            var func = eval(grid.onselectionchanged);
            new func(grid.getData()[rowIndex]);
        }

    },
    //选择一个复选框
    selecCk: function (gridIndex, rowIndex) {
        var grid = allGrid[gridIndex];
        var isChecked = document.getElementById("ck" + gridIndex + rowIndex).checked;
        selectTrOrCk = 1;
        if (isChecked) {

            $("#tr" + gridIndex + rowIndex).css("background-color", "#e2e2e2");
            grid.selectRows_[rowIndex] = true;
        }
        else {
            $("#tr" + gridIndex + rowIndex).css("background-color", "");
            delete grid.selectRows_[rowIndex];
        }

    },
    //双击
    rowdbclick: function (gridIndex, rowIndex) {
        var grid = allGrid[gridIndex];
        if (grid.dbrowclick != null && grid.dbrowclick != undefined && grid.dbrowclick != "") {
            var row = (grid.getData()[rowIndex]);
            var func = eval(grid.dbrowclick);
            new func(row);
        }
    },
    //取消选择
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
    //全选
    okSelect: function (gridIndex) {
        var grid = allGrid[gridIndex];

        $(".tr" + gridIndex).css("background-color", "#e2e2e2");
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

                        submitData($.trim(grid.url), grid.getFilterData(), function (data) {


                            grid.setData(data);

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
                        submitData($.trim(grid.url), grid.getFilterData(), function (data) {
                            grid.setData(data);

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
        setData: function (data) {
            if (allGrid.length > 0) {

                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if (!$(this).is(grid.el)) {
                        continue;
                    }
                }
                grid.data = data;
                $(grid.el).splitPager();
                $(grid.el).loadGridHtml();
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
                    var html = " <div id=\"page-grid-div" + i + "\"><table style='table-layout:fixed ;' id=\"page-grid-table" + i + "\" class=\"hbes-table\">";
                    html += " <thead>";
                    var head = "";

                    if (grid.headLoad != null && grid.headLoad != undefined && grid.headLoad != "") {
                        var func = eval(grid.headLoad);
                        head = func();
                    }
                    else {
                        head = " <tr class=\"text-c\" role=\"row\">";
                        for (var j = 0 ; j < grid.columns.length; j++) {
                            var column = grid.columns[j];
                            var width = "";
                            if ($.trim(column.width) != "") {
                                width = "width:" + $.trim(column.width) + "px;";
                            }

                            if (column.type == "indexcolumn") {
                                head += " <th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"" + width + " text-align:" + column.headAlign + "  \"></th>";
                            }
                            else if (column.type == "checkcolumn") {
                                head += " <th class=\"sorting_disabled\"  rowspan=\"1\" colspan=\"1\" style=\"" + width + " text-align:" + column.headAlign + "  \">";
                                head += "<input onclick=\"hbes.selectCkAll(" + i + ",this.checked)\" name=\"\" value=\"\" type=\"checkbox\" /></th>";
                            }
                            else if (column.type == "buttoncolumn") {
                                head += " <th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"" + width + " text-align:" + column.headAlign + "  \">操作列</th>";
                            }
                            else {
                                head += " <th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"" + width + "  text-align:" + column.headAlign + " \">";

                                head += column.name + "</th>";
                            }
                        }

                        head += "</tr>";
                    }
                    html += head;
                    //展示内容

                    html += "</thead><tbody>";
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
                            else if (column.type == "buttoncolumn") {

                                alert($("#btn").find("button").length);
                                alert($("#btn").find("span").length);
                                //alert($("#btn button").length);
                                //alert($("#btn button"));
                                $("#btn button").each(function (index, element) {
                                    var btnType = $(this).attr("btnType");
                                    var clickType = $(this).attr("clickType");
                                    html += "<td style=\"text-align:" + column.textAlign + "\"><button class=\"layui-btn btn-primary layui-btn-mini\" onclick=\"" + clickType + "\">" + btnType + "</button> </td>";
                                });

                            }
                            else {

                                var text = data[j][column.field];
                                if (text == null || text == undefined) {
                                    text = "";
                                }
                                var title = text;
                                if (column.load != null && column.load != undefined && column.load != "") {
                                    var func = eval(column.load);
                                    text = func(data[j]);
                                    title = ""
                                }
                                html += "<td title='" + title + "' style=\"text-align:" + column.textAlign + ";\" class='texttd'><p class='ut-s'>" + text + "</p></td>";
                            }
                        }

                        html += "</tr>";

                        if (grid.rowLoad != null && column.load != undefined && column.load != "") {
                            var func = eval(grid.rowLoad);
                            func(data[j], j);
                        }
                    }
                    html += "</tbody></table></div>";
                    $("#page-grid-div" + i).remove();
                    $(grid.el).append(html);
                }
            }
        },
        //选择行
        selectRow: function (rowIndex) {
            var index = 0;
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if ($(this).is(grid.el)) {
                        index = i;
                        break;
                    }
                }
                hbes.selectRow(index, rowIndex);
            }
        },
        //获取行
        getRow: function (rowIndex) {
            var index = 0;
            if (allGrid.length > 0) {
                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if ($(this).is(grid.el)) {
                        index = i;
                        break;
                    }
                }
                if (grid.data.length > rowIndex) {
                    return grid.data[rowIndex];
                }
            }
        },

        getRowIndex: function (columnName, eqValue) {
            var index = 0;
            if (allGrid.length > 0) {

                for (var i = 0; i < allGrid.length; i++) {
                    var grid = allGrid[i];
                    if ($(this).is(grid.el)) {
                        index = i;
                        break;
                    }
                }
                var rows = grid.data;
                for (var i = 0 ; i < rows.length; i++) {
                    var row = rows[i];
                    if (row[columnName] == eqValue) {
                        return i;
                    }
                }
            }
        },
        //获取选中一行
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
        //获取选中多行
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

$(function () {

    $(".hbes-tree").treeInit();

});

var allTree = new Array();
function isHideTree(inputEl, treeId) {
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
        loadSuccess: null
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
    allTree.push(obj);
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
                submitData(tree.url, data_, function (data) {

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
        }
    });
})(jQuery);

/*combox_start*/
document.onclick = function (e) {

    for (var c = 0 ; c < allCombox.length; c++) {
        var combox = allCombox[c];
        combox.isPlanHide = true;
        hbesCombox.showCombox(c);
    }
    //var clickEl = $(e.target);
    //for (var c = 0 ; c < allCombox.length; c++) {
    //    var combox = allCombox[c];
    //    var inputEl = combox.el;
    //    var iEl = $("#div_combox" + c + "_i");
    //    var optionEl = $("#div_combox_option" + c);
    //    var optionItems = $("#div_combox_option" + c + " p");

    //    if (clickEl.is(inputEl) || clickEl.is(iEl) || clickEl.is(optionEl) || optionItems.is(clickEl)) {
    //        continue;
    //    }

    //    else if (combox.multiSelect) {
    //        var optionItemsInput = $("#div_combox_option" + c + " p input");
    //        var optionItemsSpan = $("#div_combox_option" + c + " p span");

    //        if (optionItemsInput.is(clickEl) || optionItemsSpan.is(clickEl)) {
    //            continue;
    //        }
    //        combox.isPlanHide = true;
    //        hbesCombox.showCombox(c);
    //    }
    //    else {
    //        combox.isPlanHide = true;
    //        hbesCombox.showCombox(c);
    //    }
    //}
};
$(function () {
    $(".hbes-combox").comboxInit();
});


(function ($) {

    $.fn.extend({
        //初始化
        comboxInit: function () {

            var comboxArray = this;
            if (comboxArray.length > 0) {
                for (var i = 0 ; i < comboxArray.length; i++) {
                    var comboxEl = comboxArray[i];
                    createCombox(comboxEl);

                    $(comboxEl).comboxBindData();
                }
            }
        },
        comboxBindData: function (formData) {
            var index = null;
            if (allCombox.length > 0) {

                for (var i = 0; i < allCombox.length; i++) {

                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
            }

            if ($.trim(combox.url) != "") {

                var data_ = {};
                if (formData != null && formData != undefined) {
                    data_ = formData;
                }
                submitData(combox.url, data_, function (data) {

                    allCombox[index].data = data;
                    $(allCombox[index].el).showCombox();
                    // 增加加载完成事件 20170613
                    if (combox.onLoad != null && combox.onLoad != undefined) {
                        var func = eval(combox.onLoad);
                        new func();
                    }
                });
            }
            else {
                $(allCombox[index].el).showCombox();
                // 增加加载完成事件 20170613
                if (combox.onLoad != null && combox.onLoad != undefined) {
                    var func = eval(combox.onLoad);
                    new func();
                }
            }
        },
        showCombox: function () {
            var index = null;
            if (allCombox.length > 0) {

                for (var i = 0; i < allCombox.length; i++) {

                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
                el = combox.el;
                if (el.parent().is($("#div_combobx" + index))) {
                    var siblings = $(el).siblings();

                    for (var s = 0 ; s < siblings.length; s++) {

                        $(siblings[s]).remove();
                    }
                    el.unwrap();
                    $("#div_combobx" + index).remove();
                }
                var div = $("<div id='div_combobx" + index + "' class='div_combobx'></div>");
                el.attr("readonly", "readonly");

                var width = combox.width;

                var parentWidth = el.parent().width();
                if (width >= parentWidth) {
                    width = parentWidth;
                }
                div.css("width", width + "px");

                $(el).css("width", width - 25 + "px");
                $(el).wrap(div);

                var divEl = $("#div_combobx" + index);
                $(divEl).click(function (event) {
                    event.stopPropagation();

                });
                $(el).click(function (event) {
                    for (var c = 0 ; c < allCombox.length; c++) {
                        var combox = allCombox[c];
                        combox.isPlanHide = true;
                        hbesCombox.showCombox(c);
                    }
                    hbesCombox.clickCombox(index);
                    event.stopEvent();
                });

                var height = ($(el).height());

                if (combox.readonly == "readonly") {
                    divEl.append("<i class='layui-icon' id='div_combox" + index + "_i'   style='display:none'>&#xe625;</i>");
                } else {
                    divEl.append("<i class='layui-icon' id='div_combox" + index + "_i'   style='font-size:14px;height:"
                    + height + "px;line-height:" + height + "px;'>&#xe625;</i>");
                }

                $("#div_combox" + index + "_i").click(function () {
                    for (var c = 0 ; c < allCombox.length; c++) {
                        var combox = allCombox[c];
                        combox.isPlanHide = true;
                        hbesCombox.showCombox(c);
                    }
                    hbesCombox.clickCombox(index);

                });
                var options = "";

                if (combox.showNullItem && !combox.multiSelect) {

                    var tmpData = combox.data;
                    combox.data = [];
                    var valueField = combox.valueField;
                    var textField = combox.textField;
                    var nullOption = "{'" + textField + "':'请选择','" + valueField + "':''}";
                    if (tmpData.length == 0 || tmpData[0][combox.valueField] != "") {
                        combox.data.push(eval('(' + nullOption + ')'));
                    }

                    for (var i = 0 ; i < tmpData.length; i++) {

                        combox.data.push(tmpData[i]);
                    }

                }

                for (var i = 0 ; i < combox.data.length; i++) {
                    var option = combox.data[i];

                    if (!combox.multiSelect) {
                        options += "<p class='div_combox_option" + index + "_p" + "' value_='" + option[combox.valueField] +
                            "' onclick=\"hbesCombox.selectOption(" + index + ",'" + option[combox.valueField] +
                            "')\" id='div_combox_option" + index + "_p" + option[combox.valueField] + "'>" +
                            option[combox.textField] + "</p>";
                    }
                    else {
                        options += "<p class='div_combox_option" + index + "_p" +
                            "' onclick=\"hbesCombox.selectOption(" + index + ",'" + option[combox.valueField] +
                            "')\" id='div_combox_option" + index + "_p" + option[combox.valueField] + "'>" +
                           " <input value='" + option[combox.valueField] + "' class='div_combox_option_" + index
                           + "_cb' id='div_combox_option_" + index + "_cb_" + option[combox.valueField] + "' type='checkbox' /><span>"
                           + option[combox.textField] + "</span></p>";
                    }
                }
                divEl.append("<div   id='div_combox_option" + index + "' class='div_combox_option'>" + options + "</div>");

                $(".div_combox_option_" + index + "_cb").click(function (event) {
                    this.checked = !(this.checked);
                });
                var div_combox_option = $("#div_combox_option" + index);

                div_combox_option.css("width", width + "px");
                if (combox.maxHeight != undefined && combox.maxHeight != null) {

                    div_combox_option.css("max-height", combox.maxHeight + "px");
                }
                div_combox_option.hide();
                hbesCombox.showComboxContext(index);
            }
        },
        comboxSetValue: function (value) {
            var index = null;
            if (allCombox.length > 0) {

                for (var i = 0; i < allCombox.length; i++) {

                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
                combox.value = value;
                hbesCombox.showComboxContext(index);
            }
        },
        comboxGetValue: function () {
            var index = null;
            if (allCombox.length > 0) {

                for (var i = 0; i < allCombox.length; i++) {

                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
                return combox.value;
            }
        },
        comboxSetData: function (data) {
            if (allCombox.length > 0) {

                for (var i = 0; i < allCombox.length; i++) {

                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
                combox.data = data;
                $(combox.el).showCombox();
            }
        },
        valiCombox: function () {
            if (allCombox.length > 0) {

                for (var i = 0; i < allCombox.length; i++) {

                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return true;
                }
                var combox = allCombox[index];
                if (combox.vtype == "required" && combox.value == "") {
                    $("#div_combobx" + index).css("border", "1px solid red");
                    $("#div_combobx" + index).attr("title", "不能为空");
                    return false;
                }
                else {
                    $("#div_combobx" + index).css("border", "");
                    $("#div_combobx" + index).attr("title", "");
                    return true;
                }
            }
        },

        // 清空下拉框选项 20170621 by ylb  
        clear: function () {
            var index = null;
            if (allCombox.length > 0) {
                for (var i = 0; i < allCombox.length; i++) {
                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
                combox.data = [];
                combox.value = "";
                $(combox.el).showCombox();
            }
        },

        // 增加load方法 20170612 by ylb
        load: function (url, callback) {
            var index = null;
            if (allCombox.length > 0) {
                for (var i = 0; i < allCombox.length; i++) {
                    var combox = allCombox[i];
                    if ($(this).is(combox.el)) {
                        index = i;
                        break;
                    }
                }
                if (index == null) {
                    return;
                }
                var combox = allCombox[index];
                combox.url = url;
                submitData(url, null, function (data) {
                    allCombox[index].data = data;
                    allCombox[index].value = "";
                    $(allCombox[index].el).showCombox();
                    if (callback != null && callback != undefined) {
                        callback([]);
                    }
                });
            }
        }
    })
})(jQuery);


var allCombox = new Array();
function createCombox(comboxEl) {
    var obj = new Object();
    obj =
        {
            el: $(comboxEl),
            url: "",
            valueField: "id",
            textField: "text",

            showNullItem: false,
            data: [],
            width: 100,
            maxHeight: 200,
            value: "",
            text: "",
            vtype: "",
            multiSelect: false,
            isPlanHide: true,
            onValueChanged: null,     // 增加选中值变化事件 20170612 by ylb
            onLoad: null,                 //增加加载完成事件 20170613 by ylb
            readonly: null           //增加readonly属性 20170613 readonly="readonly"表示只读  不加readonly属性即不只读  by ylb
        }

    //// 增加选中值变化事件 20170612
    var onValueChanged = $(comboxEl).attr("onValueChanged");
    if (onValueChanged != undefined) {
        obj.onValueChanged = onValueChanged;
    }
    //增加加载完成事件 20170613
    var onLoad = $(comboxEl).attr("onLoad");
    if (onLoad != undefined) {
        obj.onLoad = onLoad;
    }

    //增加readonly属性 20170613
    var _readonly = $(comboxEl).attr("readonly");
    if (_readonly != undefined) {
        obj.readonly = _readonly;
    }

    var url = $(comboxEl).attr("url");
    if (url != undefined) {
        obj.url = url;
    }
    obj.width = obj.el.width();

    var valueField = $(comboxEl).attr("valueField");
    if (valueField != undefined) {
        obj.valueField = valueField;
    }
    var textField = $(comboxEl).attr("textField");
    if (textField != undefined) {
        obj.textField = textField;
    }

    var maxHeight = $(comboxEl).attr("maxHeight");
    if (maxHeight != undefined) {
        obj.maxHeight = maxHeight;
    }

    var value = $(comboxEl).attr("value");
    if (value != undefined) {
        obj.value = value;
    }
    var showNullItem = $(comboxEl).attr("showNullItem");
    if (showNullItem != undefined) {
        if (showNullItem == "true") {
            obj.showNullItem = true;
        }
    }
    var vtype = $(comboxEl).attr("vtype");
    if (vtype != undefined) {
        obj.vtype = vtype;
    }
    var data = $(comboxEl).attr("data");
    if (data != undefined) {
        obj.data = eval('(' + data + ')');
    }
    var multiSelect = $(comboxEl).attr("multiSelect");
    if (multiSelect != undefined) {
        if (multiSelect == "true") {
            obj.multiSelect = true;
        }
    }
    allCombox.push(obj);
}
var hbesCombox =
{
    clickCombox: function (index) {
        var combox = allCombox[index];
        //增加readonly属性 20170613
        if (combox.readonly != null && (combox.readonly == "readonly")) {
            return;
        }
        if (combox.isPlanHide) {
            combox.isPlanHide = false;
        }
        else {
            combox.isPlanHide = true;
        }

        hbesCombox.showCombox(index);

    },
    showCombox: function (index) {

        var combox = allCombox[index];

        if (combox.isPlanHide) {
            $("#div_combox_option" + index).hide();
        }
        else {
            $("#div_combox_option" + index).show();
        }
    },
    showComboxContext: function (index) {

        var combox = allCombox[index];
        var valArray = combox.value.split(",");
        var text = "";
        if (valArray.length > 0) {
            $("#div_combox_option" + index + " p").css("background", "");
            for (var v = 0 ; v < valArray.length; v++) {
                var val = valArray[v];
                for (var i = 0 ; i < combox.data.length; i++) {
                    var option = combox.data[i];

                    if (option[combox.valueField] == val) {

                        text += option[combox.textField] + ",";
                        var id = "div_combox_option" + index + "_p" + val;
                        $("#" + id).css("background", "#1E9FFF");
                        if (combox.multiSelect) {
                            var ckItems = $(".div_combox_option_" + index + "_cb");
                            for (var i = 0 ; i < ckItems.length; i++) {
                                if (ckItems[i].value == val) {
                                    ckItems[i].checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (text != "") {
            text = text.substring(0, text.length - 1);
        }
        else {
            combox.value = "";
        }
        combox.el.val(text);
    },
    selectOption: function (index, value) {

        var combox = allCombox[index];
        if (!combox.multiSelect) {
            combox.isPlanHide = true;
            combox.value = value;
            hbesCombox.showComboxContext(index);
            hbesCombox.showCombox(index);
        }
        else {
            combox.isPlanHide = false;

            hbesCombox.showComboxContext(index);
            hbesCombox.showCombox(index);
            var ck = document.getElementById("div_combox_option_" + index + "_cb_" + value);

            if (ck.checked) {
                ck.checked = false;
            }
            else {
                ck.checked = true;
            }

            hbesCombox.clickCb(index, value);
        }
        $(combox.el).valiCombox();
        // 增加选中值变化事件 20170612
        if (combox.onValueChanged != null && combox.onValueChanged != undefined) {
            var func = eval(combox.onValueChanged);
            new func(value);
        }
    },
    clickCb: function (index, value) {

        var ckItems = $(".div_combox_option_" + index + "_cb");
        var val = "";

        for (var i = 0 ; i < ckItems.length; i++) {
            if (ckItems[i].checked) {
                val += ckItems[i].value + ",";
            }
        }
        if (val != "") {
            val = val.substring(0, val.length - 1);
        }

        allCombox[index].value = val;
        hbesCombox.showComboxContext(index);
        $(combox.el).valiCombox();
    }

};
/*combox_end*/


$(function () {

    $(".hbes-input").blur(function () {
        $(this).valiInput();
    });

    // $("#no-null").valiInput();
});
function valiType(vtype, el) {
    var resultObj = new Object();
    resultObj.isPass = false;
    resultObj.msg = "";
    if (vtype == null || vtype == undefined || $.trim(vtype) == "") {
        resultObj.isPass = true;
        return resultObj;
    }
    var valiArray = vtype.split(";");
    if (valiArray != null && valiArray != undefined && valiArray.length > 0) {
        for (var v = 0 ; v < valiArray.length; v++) {
            var vali = $.trim(valiArray[v]);
            if (vali != "") {
                var valiMap = vali.split(":");

                var valiKey = $.trim(valiMap[0]);
                var valiValue = $.trim(valiMap[1]);

                if (valiKey != null && valiKey != undefined && valiKey != "" && valiValue != null && valiValue != undefined && valiValue != "") {
                    switch (valiKey) {
                        case "required":
                            if (valiValue == "true") {
                                if (el.val().trim() == "") {
                                    resultObj.isPass = false;
                                    resultObj.msg = "不能为空";
                                    return resultObj;
                                }
                            }
                            break;
                        case "maxLength":
                            if (el.val() != "" && el.val().length > valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "长度不能超过" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "minLength":
                            if (el.val() != "" && el.val().length < valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "长度不能少于" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "rangeLength":
                            var startRange = valiValue.split(",")[0];
                            var endRange = valiValue.split(",")[1];
                            if (el.val() != "" && (el.val().length < startRange || el.val().length > endRange)) {
                                resultObj.isPass = false;
                                resultObj.msg = "字符长度必须在" + startRange + "到" + endRange + "之间";
                                return resultObj;
                            }
                            break;
                        case "max":
                            if (el.val() != "" && parseFloat(el.val()) > valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "最大值为" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "min":
                            if (el.val() != "" && parseFloat(el.val()) < valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "最小值为" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "range":
                            var startRange = valiValue.split(",")[0];
                            var endRange = valiValue.split(",")[1];
                            if (el.val() != "" && parseFloat(el.val()) < startRange || parseFloat(el.val()) > endRange) {
                                resultObj.isPass = false;
                                resultObj.msg = "值的范围在" + startRange + "到" + endRange + "之间";
                                return resultObj;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (valiKey != null && valiKey != undefined && valiKey != "") {

                    switch (valiKey) {
                        case "int":
                            var test = /^-?\d+$/;
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入整数";
                                return resultObj;
                            }
                            break;
                        case "email":
                            var test = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入邮件格式";
                                return resultObj;
                            }
                            break;
                        case "url":
                            var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
                         + "?(([0-9a-z_!~*'().&amp;=+$%-]+: )?[0-9a-z_!~*'().&amp;=+$%-]+@)?" //ftp的user@
                         + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184
                         + "|" // 允许IP和DOMAIN（域名）
                         + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.
                         + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名
                         + "[a-z]{2,6})" // first level domain- .com or .museum
                         + "(:[0-9]{1,4})?" // 端口- :80
                         + "((/?)|" // a slash isn't required if there is no file name
                         + "(/[0-9a-z_!~*'().;?:@&amp;=+$,%#-]+)+/?)$";
                            var test = new RegExp(strRegex);
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入url格式";
                                return resultObj;
                            }
                            break;
                        case "float":
                            var test = /^(-?\d+)(\.\d+)?$/;
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入数字";
                                return resultObj;
                            }
                            break;
                    }
                }


            }
        }
        resultObj.isPass = true;
        return resultObj;
    }
    else {
        resultObj.isPass = true;
        return resultObj;
    }
}

(function ($) {
    $.fn.extend({
        valiInput: function () {
            var vtype = $(this).attr("vtype");
            var myVali = $(this).attr("onvalidation");

            var obj;

            if (myVali != null && myVali != undefined) {

                var func = eval(myVali);
                obj = new func($(this));
            }
            else {
                obj = valiType(vtype, $(this));
            }
            if (!obj.isPass) {
                $(this).css("border", "1px solid red");
                $(this).attr("title", obj.msg);
            }
            else {
                $(this).css("border", "");
                $(this).attr("title", "");
            }
            return obj.isPass;
        },
        valiForm: function () {
            var form_control_array = $(this).find(".hbes-input");
            var isPass = true;
            if (form_control_array != null && form_control_array != undefined && form_control_array.length > 0) {

                for (var i = 0 ; i < form_control_array.length; i++) {
                    var control = form_control_array[i];

                    if (!$(control).valiInput()) {
                        isPass = false;
                    };
                }
            }
            if (allCombox != null && allCombox != undefined && allCombox.length > 0) {
                for (var i = 0 ; i < allCombox.length; i++) {
                    var combox = allCombox[i].el;
                    if (!$(combox).valiCombox()) {
                        isPass = false;
                    }
                }
            }
            return isPass;
        },
        valiLayUIForm: function () {
            var form_control_array = $(this).find(".layui-input");
            var isPass = true;
            if (form_control_array != null && form_control_array != undefined && form_control_array.length > 0) {

                for (var i = 0 ; i < form_control_array.length; i++) {
                    var control = form_control_array[i];

                    if (!$(control).valiInput()) {
                        isPass = false;
                    };
                }
            }
            if (allCombox != null && allCombox != undefined && allCombox.length > 0) {
                for (var i = 0 ; i < allCombox.length; i++) {
                    var combox = allCombox[i].el;
                    if (!$(combox).valiCombox()) {
                        isPass = false;
                    }
                }
            }
            return isPass;
        }
    })
})(jQuery);