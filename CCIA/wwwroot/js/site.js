/**
 * @param  {string} name
 * @param  {string} insertVal
 * @param  {string} formId
 */
function insertHiddenInput(name, insertVal, formId) {
    let inputInsert = document.createElement("input");
    inputInsert.setAttribute("type", "hidden");
    inputInsert.setAttribute("name", name);
    inputInsert.setAttribute("value", insertVal.toString());
    document.getElementById(formId).appendChild(inputInsert); 
}

/**
 * 
 * @param {*} element 
 * @param {Array} array 
 */
function countOccurrences(element, array) {
    let count;
    for (let item of array) {
        if (item === element) {
            count++;
        }
    }
    return count;
}
