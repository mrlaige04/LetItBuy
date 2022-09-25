var category = {
    categoryName: "aboba",
    characteristics: [],
};

function appendField() {
    var div = document.createElement('div');
    div.className = "criteriaForms";
    var divInner = document.createElement('div');
    divInner.className = "d-flex justify-content-center";
    var inputNameField = document.createElement('input');
    inputNameField.name = "criteriaName";
    inputNameField.className = "form-control me-2";
    inputNameField.type = "text";
    inputNameField.placeholder = "Characteristic name";
    inputNameField.required = true;
    divInner.appendChild(inputNameField);
    var select = document.createElement('select');
    select.required = true;
    select.className = "form-select";
    select.name = "criteriaType";
    select.innerHTML = `@Html.DropDownList("New field", criteriaTypes, new { @class = "form-control", @name = "criteriaType", @required = "required" })`;
    divInner.appendChild(select);
    div.innerHTML = "<label for='itemName'>New field</label>";
    div.appendChild(divInner);
    document.getElementById('characteristics').appendChild(div);
}