var counter;
var trigers;
var emptyTriggerIndex;
var containerClass;
var inputsClass;
var formId;
function Duplicator(conClass, inpClass, foId) {
    containerClass = conClass;
    inputsClass = inpClass;
    formId = foId;
    const EMPTY_NODE = 0;
    const FILLED_NODE = 1;
    const NOT_AVAILABLE_NODE = 2;
$(document).ready(function () {
    var node = $("." + containerClass)[0];
    var emptyNodeClone = node.cloneNode(true)
    trigers = [0]
    emptyTriggerIndex = EMPTY_NODE
    counter = parseInt(node.getAttribute("index"))

    function inputListener() {
        var index = parseInt(this.getAttribute("index"))
        console.log(analizeFields(this))
        switch (analizeFields(this)) {
            case 0:
                redistributeTriggers(index , EMPTY_NODE)
                break;
            case 1:
                redistributeTriggers(index, FILLED_NODE)
                break;
        }
    }

    node.addEventListener("input", inputListener)
    for (var i = 0; i < node.children.length; i++) {
        if (node.children[i].type == "file") {
            node.children[i].addEventListener("change", function () { 
            var event = document.createEvent("Event")
            event.initEvent("input")
            node.dispatchEvent(event)
            })
        }
    }

    function analizeFields(node) {
        var inputs = node.getElementsByClassName(inputsClass)
        var counterString = "";
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "file") {
                if (inputs[i].value.length > 0) {
                    counterString += "!"
                } else {
                    counterString += ""
                }
            } else {
                counterString += inputs[i].value
            }
            
        }
        var formatedString = counterString.replace(/\s/g, '');
        return counterString.length;
    }

    function redistributeTriggers(index, method) {
        switch (method) {
            case 1:
                if (emptyTriggerIndex == index) {
                    trigers[index] = FILLED_NODE
                    trigers.push(EMPTY_NODE);
                    AddNode();
                    emptyTriggerIndex = counter;
                }
                break;
            case 0:
                trigers[index] = EMPTY_NODE
                trigers[emptyTriggerIndex] = NOT_AVAILABLE_NODE;
                RemoveNode(emptyTriggerIndex)
                emptyTriggerIndex = index;
                break;
            default:

        }
    }

    function AddNode() {
        var lastNode = $("." + containerClass)[$("." + containerClass).length - 1];
        counter++;
        var newNode = BuildNode();
        lastNode.after(newNode)
    }

    function RemoveNode(index) {
        var removedNode = $('.' + containerClass + '[index=' + index + ']')[0]
        removedNode.remove();
    }

    function BuildNode() {
        var clone = emptyNodeClone.cloneNode(true);
        clone.setAttribute("index", counter);
        clone.addEventListener("input", inputListener)
        for (var i = 0; i < clone.children.length; i++) {
            if (clone.children[i].type == "file") {
                clone.children[i].addEventListener("change", function () {
                    var event = document.createEvent("Event")
                    event.initEvent("input")
                    clone.dispatchEvent(event)
                })
            }
        }
        return clone
    }



    function getJSON() {
        prepareFieldsBeforeSend();
    }

    $("#"+formId).submit(function () {
        prepareFieldsBeforeSend();
    })
    function rebuild(){
        prepareFieldsBeforeSend();
    }
})
}

function rebuild() {
    prepareFieldsBeforeSend();
}

function prepareFieldsBeforeSend() {
    var redistributedCounter = 0;
    trigers.forEach(function (node, index) {
        if (node == 1) {
            var currentNode = $('.' + containerClass + '[index=' + index + ']')[0];
            var nodeInputs = currentNode.getElementsByClassName(inputsClass);
            for (var i = 0; i < nodeInputs.length ; i++) {
                nodeInputs[i].name = nodeInputs[i].name.replace(/\[(.*?)\]/, "[" + redistributedCounter + "]")
                console.log(nodeInputs[i].name)
            }
            currentNode.setAttribute("index", redistributedCounter);
            redistributedCounter++;
        }
    })
    counter = redistributedCounter;
    return true;
}