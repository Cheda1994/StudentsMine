    function test() {
        var inputs = $(".data");
        var names = [];
        var parent;
        var json = {};
        inputs.each(function (index, node) {
            names.push(node.name.split('%'))
        })
        console.log(names)
        names.forEach(function (elements, index) {
            elements.forEach(function (element, position) { 
                switch (position) {
                    case 0:
                        var a_v = element.split('.');
                        if (a_v.length == 1) {
                            if (json[a_v[0]] == undefined) {
                                if (elements.length == position + 1) {
                                    if (inputs[index].type == 'checkbox') {
                                        json[a_v[0]] = inputs[index].checked;
                                    }else{
                                        json[a_v[0]] = inputs[index].value;
                                    }
                                } else {
                                    json[a_v[0]] = {};
                                }
                            }
                            parent = json[a_v[0]];
                        } else if (a_v.length == 2) {
                            var arrayPosition = parseInt(a_v[0].match(/\[(.*?)\]/)[1])
                            if (arrayPosition == 0) {
                                if (json[a_v[1]] == undefined) {
                                    json[a_v[1]] = [];
                                }
                            }
                                if (elements.length == position + 1) {
                                    json[a_v[1]].push(inputs[index].value)
                                } else {
                                    if (json[a_v[1]][arrayPosition] == undefined) {
                                        json[a_v[1]][arrayPosition] = {};
                                        parent = json[a_v[1]][arrayPosition];
                                    } else {
                                        parent = json[a_v[1]][arrayPosition];
                                    }
                                }
                            
                        }
                    break;
                default:
                    var a_v = element.split('.');
                    if (a_v.length == 1) {
                        if (parent[a_v[0]] == undefined) {
                            if (elements.length == position + 1) {
                                if (inputs[index].type == 'checkbox') {
                                    parent[a_v[0]] = inputs[index].checked;
                                }else{
                                    parent[a_v[0]] = inputs[index].value;
                                }
                            } else {
                                parent[a_v[0]] = {};
                            }
                        }
                        parent = parent[a_v[0]];
                    } else if (a_v.length == 2) {
                        var arrayPosition = parseInt(a_v[0].match(/\[(.*?)\]/)[1])
                        if (arrayPosition == 0) {
                            if (parent[a_v[1]] == undefined) {
                                parent[a_v[1]] = [];
                            }
                        }
                            if (elements.length == position + 1) {
                                parent[a_v[1]].push(inputs[index].value)
                            } else {
                                if (parent[a_v[1]][arrayPosition] == undefined) {
                                    parent[a_v[1]][arrayPosition] = {};
                                    parent = parent[a_v[1]][arrayPosition];
                                } else {
                                    parent = parent[a_v[1]][arrayPosition];
                                }
                            }
                    }
                    break;
            }
            })
        })
        return json;
    }