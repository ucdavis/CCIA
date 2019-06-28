/**
 * @param  {} name
 * @param  {} insertVal
 * @param  {} formId
 */
function insertHiddenInput(name, insertVal, formId) {
    let inputInsert = document.createElement("input");
    inputInsert.setAttribute("type", "hidden");
    inputInsert.setAttribute("name", name);
    inputInsert.setAttribute("value", insertVal.toString());
    document.getElementById(formId).appendChild(inputInsert); 
}
