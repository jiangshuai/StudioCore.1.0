﻿layui.config({
    base: '/Content/lib/system/'
}).use(['jquery', 'element', 'layer', 'form', 'common'], function () {
    var $ = layui.$, layer = layui.layer, form = layui.form, element = layui.element, common = layui.common;

    common.$sideShrink("#LAY_app");

    });

function setPwd() {
    layer.open({
        type: 2,
        title: '修改密码',
        shadeClose: true,
        shade: false,
        maxmin: true,
        area: ['560px', '360px'],
        content: '/Users/SetPwd'
    });
}
