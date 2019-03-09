(function ($) {
    //Com_ztree_param = {
    //    //放置 树 的 select
    //    comzTree_SelectId: "ComZTree",
    //    //树 id
    //    comzTree_Id: "ComZTree_zTree",
    //    //树显示值
    //    data_key_name: "text",
    //    //主见值
    //    data_simpleData_idKey: "id",
    //    //上级 值
    //    data_simpleData_pIdKey: "Pid",
    //    //默认根节点 的 值
    //    data_simpleData_rootPid: "0",
    //    //默认值 选择节点 （id 值） 一般用于修改赋值,初始化为 ""
    //    defaultValue: ["3"],// [], //
    //    //是否显示复选  初始化 为false
    //    isCheckBox: false,
    //    //树对象 $.fn.zTree.getZTreeObj(ZTree_Obj.comzTree_Id);
    //    zTrees: null,
    //    //默认是否展开
    //    isExpand: true,
    //    //获取 tree 数据的 url
    //    getTreeData_Url: "../ashx/zTree.ashx"
    //};

    $.fn.extend({
        ComZTree_Init: function (ZTree_Obj) {
            var TreeHtmlConrleID = ZTree_Obj.comzTree_SelectId
            hbes_BindZTreeObj.addTreeControl(TreeHtmlConrleID);
            hbes_BindZTreeObj.GetTreeObj(ZTree_Obj);
            /*单击 显示/隐藏 ztree*/
            $("#" + TreeHtmlConrleID).toggle(function () {
                $("#" + TreeHtmlConrleID + "_zTree").show();
            }, function () {
                $("#" + TreeHtmlConrleID + "_zTree").hide();
            })
            /*树失去焦点，隐藏 tree*/
            $(".ztree>li").blur(function () {
                $(this).hide();
            });
        },
        //获取 树复选框选择的 项
        GetTreeChecked: function (ZTree_Obj) {
            var vlu = "";
            var treeOp = Com_ztree_param.zTrees;
            var nodes = treeOp.getCheckedNodes(true);
            for (var i = 0, j = nodes.length; i < j; i++) {
                if (vlu != "") { vlu += ","; }
                vlu += nodes[i].id;
            }
            return vlu;
        },
        //获取 选择项
        GetTreeSelectNode: function (ZTree_Obj) {
            var selectvlu = "";
            var treeOp = Com_ztree_param.zTrees;
            var nodes = treeOp.getSelectedNodes();
            return nodes;
        }
    });
})(jQuery);

var hbes_BindZTreeObj = {
    //向页面添加 ZTree html载体控件(TreeHtmlConrleID)，并控制位置 宽度
    addTreeControl: function (TreeHtmlConrleID) {

        var mt_top = $("#" + TreeHtmlConrleID).offset().top;
        var mt_left = $("#" + TreeHtmlConrleID).offset().left;
        mt_top += $("#" + TreeHtmlConrleID).height() + 14;
        //获取控件的宽度
        var Control_Width = $("#" + TreeHtmlConrleID).width();

        var treeid = TreeHtmlConrleID + "_zTree";
        $("body").append("<ul class='ztree' id='" + treeid + "' name='" + treeid +
            "' style=\"position: absolute;width:" + (Control_Width + 43) + "px;top: "
            + (mt_top) + "px; left: " + mt_left + "px;\"></ul>");

        $("#" + treeid).css({
            "border": "1px solid #617775",
            "background": "#F0F6E4",
            "height": "auto",
            "maxheight": "200px",
            "overflow-y": "scroll",
            "overflow-x": "auto",

        });
        $("#" + treeid).hide();
    },
    //绑定树
    BindComZtreeNodeFun: function (ZTree_Obj) {
        return hbes_BindZTreeObj.Setting(ZTree_Obj);
    },
    /*给 select 赋值 ，htmlId：下拉框id, vlaue：id值, text：显示值  单个*/
    SetValueSelect: function (treeid, value, text) {
        var el = document.getElementById(treeid);
        el.value = text;

    },

    Setting: function (ZTree_Obj) {
        var htmlTreeId = ZTree_Obj.comzTree_SelectId;
        var keyID = ZTree_Obj.data_simpleData_idKey;
        var setting = {
            check: {
                enable: ZTree_Obj.isCheckBox,//启用
                chkStyle: "checkbox",
                autoCheckTrigger: true,//自动触发回掉函数
                chkboxType: { "Y": "", "N": "" }
            },
            data: {
                key: {
                    name: ZTree_Obj.data_key_name
                },
                simpleData: {
                    enable: true,
                    idKey: keyID,//节点绑定的字段(数据库表字段groupid)
                    pIdKey: ZTree_Obj.data_simpleData_pIdKey,//节点的上级字段(数据库表字段parentid)
                    rootPId: ZTree_Obj.data_simpleData_rootPid// 根id
                }
            },
            async: {
                enable: true
            },
            callback: {
                beforeClick: function () { },
                onCheck: function (e, treeid, treeNode) {
                    //   alert("sdf");
                    //
                },
                onClick: function (event, htmlTreeId, treeNode) {
                    //获取树对象
                    var zTree_Menu = $.fn.zTree.getZTreeObj(htmlTreeId);
                    /*单击 节点 选择 复选框*/
                    if (ZTree_Obj.isCheckBox) {
                        zTree_Menu.checkNode(treeNode, !treeNode.checked, true);
                    }
                    //treeNode.ModuleId
                    var node = zTree_Menu.getNodeByParam("id", treeNode.id);
                    //指定选中ID节点展开   
                    zTree_Menu.expandNode(node, true, false);

                    /*1.判断 该节点 是否还有下级节点，
                      2.如果有， 继续展开，
                      3.没有，选择单击的项，并且 关闭 树children
                    */
                    var srcNode = zTree_Menu.getSelectedNodes();
                    var Children_Arr = srcNode[0].children;
                    if (Children_Arr != undefined && Children_Arr.length > 0) {
                        // 继续展开
                        $("#" + htmlTreeId).hide();
                    } else {
                        //没有下级菜单，隐藏 ztree
                        $("#" + htmlTreeId).hide();
                    }

                    /*给 select 赋值 ，htmlId：下拉框id, vlaue：id值, text：显示值*/
                    hbes_BindZTreeObj.SetValueSelect(ZTree_Obj.comzTree_SelectId, treeNode.id, treeNode.text);


                },
                beforeExpand: function () { },
                onAsyncSuccess: function (event, htmlTreeId, treeNode, msg) {

                }
            },

            view: {
                //此处设置字体样式等 
                fontCss: { "font-size": ZTree_Obj.view_font_size = "" ? "14px" : ZTree_Obj.view_font_size },
            }
        }
        return setting;
    },
    //选择、展开 指定的节点
    SelectNodeByParamid: function (ZTree_Obj) {
        var zTree_Menu1 = Com_ztree_param.zTrees;// $.fn.zTree.getZTreeObj(ZTree_Obj.comzTree_Id);
        if (zTree_Menu1) {
            var param_ids = ZTree_Obj.defaultValue;
            var selectId = "";
            var selectText = "";
            if (param_ids != null && param_ids.length > 0) {
                for (var i = 0, j = param_ids.length; i < j; i++) {
                    if (param_ids[i] != null && param_ids[i] != undefined && param_ids[i] != "") {
                        var node = zTree_Menu1.getNodeByParam("id", param_ids[i], null);
                        if (node != null && node != undefined) {
                            selectText += (selectText == "") ? node.text : "," + node.text;
                            selectId += (selectId == "") ? node.id : "," + node.id;
                            zTree_Menu1.selectNode(node, true);//指定选中ID的节点  
                            zTree_Menu1.expandNode(node, true, false);//指定选中ID节点展开  
                            //复选框 勾选
                            if (ZTree_Obj.isCheckBox) {
                                zTree_Menu1.checkNode(node, !node.checked, true);
                            }
                        }
                    }

                }
                hbes_BindZTreeObj.SetValueSelect(ZTree_Obj.comzTree_SelectId, selectId, selectText);
            }
        } else {
            alert("树对象 为 null");
        }
    },
    //绑定 tree
    GetTreeObj: function (ZTree_Obj) {
        //初始化 树  的 参数 配置（setting）
        setting = hbes_BindZTreeObj.BindComZtreeNodeFun(Com_ztree_param);

        $.ajax({
            url: ZTree_Obj.getTreeData_Url,
            async: false,
            dataType: "json",
            type: "post",
            success: function (datas) {
                //获取 树 对象
                var zTreeObj = $.fn.zTree.init($("#" + Com_ztree_param.comzTree_Id), setting, datas);
                zTreeObj.expandAll(ZTree_Obj.isExpand);//默认是否展开
                //给树 对象 赋值
                Com_ztree_param.zTrees = zTreeObj;
                //if (Com_ztree_param.defaultValue != null && Com_ztree_param.defaultValue != undefined && Com_ztree_param.defaultValue.length > 0) {
                //    hbes_BindZTreeObj.SelectNodeByParamid(Com_ztree_param);
                //}
            }
        });
        //submitData(ZTree_Obj.getTreeData_Url, {}, function (datas) {
        //    //获取 树 对象
        //    var zTreeObj = $.fn.zTree.init($("#" + Com_ztree_param.comzTree_Id), setting, datas);
        //    zTreeObj.expandAll(ZTree_Obj.isExpand);//默认是否展开
        //    //给树 对象 赋值
        //    Com_ztree_param.zTrees = zTreeObj;
        //    if (Com_ztree_param.defaultValue != null && Com_ztree_param.defaultValue != undefined && Com_ztree_param.defaultValue.length > 0) {
        //        hbes_BindZTreeObj.SelectNodeByParamid(Com_ztree_param);
        //    }
        //});

    }
}