function insertHiddenInput(name, insertVal, formId) {
    var inputInsert = $("<input>")
        .attr("type", "hidden")
        .attr("name", name).val(insertVal);
    $(formId).append(inputInsert); 
}