﻿
$(".edit-btn").click(editFood);
$(".search-res-add-btn").click(quickAddFood);
$(".edit-weight").click(editWeight);

function editFood() {
    var parentForm = $(this).parent();
    $(this).remove();
    parentForm.append("<button class=\"btn update-btn\" type=\"submit\" value=\"submit\">Update</button>");
    parentForm.append("<tr class=\"edit-meal-props\"></td><td><div class=\"updateFood-properties\">" +
        "<div class=\"mealClass-field\">" +
        "<label>Meal Classification</label>" +
        "<select id=\"mealUpdate\" class=\"form-control\" name =\"mealId\">" +
        "<option value=\"1\">Breakfast</option>" +
        "<option value =\"2\">Lunch</option>" +
        "<option value =\"3\">Dinner</option>" +
        "<option value =\"4\">Snack</option>" +
        "<option value =\"5\">Dessert</option>" +
        "</select > "
        + "</div >" +
        "<div class=\"numberOfServings-field\">" +
        "<label>Number of Servings: </label>" +
        "<select id=\"numOfServUpdate\" class=\"form-control\" name=\"numberOfServings\">" +
        "<option value =\"1\">1</option> " +
        "<option value =\"2\">2</option> " +
        "<option value =\"3\">3</option> " +
        "<option value =\"4\">4</option> " +
        "<option value =\"5\">5</option> " +
        "<option value =\"6\">6</option> " +
        "<option value =\"7\">7</option> " +
        "<option value =\"8\">8</option> " +
        "<option value =\"9\">9</option> " +
        "<option value =\"10\">10</option>" +
        "</select >" +
        "</div></div></td></tr>");
    addSelectedOption();
    return false;
}

function addSelectedOption() {
    let mealSelection = document.getElementById('mealUpdate').parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.firstElementChild.innerText;
    let mealSelectionOptions = parseInt(document.getElementById('mealUpdate').childElementCount,10);
    let mealSelectionSelect = document.getElementById('mealUpdate');
    let mealSelectionChild = mealSelectionSelect.firstElementChild;
    for (let m = 0; m < mealSelectionOptions; m++) {
        if (mealSelectionChild.innerText != mealSelection) {
            mealSelectionChild = mealSelectionChild.nextElementSibling;
        }
        else {
            break;
        }
    }
    mealSelectionChild.setAttribute('selected', 'selected');

    let servingSelection = parseInt(document.getElementById('numOfServUpdate').parentElement.parentElement.parentElement.firstElementChild.nextElementSibling.innerText, 10);
    let servingDropDownSelect = document.getElementById('numOfServUpdate').firstElementChild;
    for (let i = 1; i < servingSelection; i++) {
        servingDropDownSelect = servingDropDownSelect.nextElementSibling;
       
    }
    servingDropDownSelect.setAttribute('selected', 'selected');
}

function quickAddFood() {
    var parentForm = $(this).parent()
    $(this).remove();
    parentForm.append("<button class=\"btn btn-info search-res-add-btn\" type=\"submit\">Add Food</button></div>");
    parentForm.append("<div class=\"search-res-add-popup\"><label>Meal Classification</label><select class=\"form-control\" name=\"mealId\"><option value=\"1\">Breakfast</option><option value=\"2\">Lunch</option><option value=\"3\">Dinner</option><option value=\"4\">Snack</option><option value=\"5\">Dessert</option></select><label>Number of Servings: </label><select class=\"form-control\" name=\"numberOfServings\"><option value=\"1\">1</option><option value=\"2\">2</option><option value=\"3\">3</option><option value=\"4\">4</option><option value=\"5\">5</option><option value=\"6\">6</option><option value=\"7\">7</option><option value=\"8\">8</option><option value=\"9\">9</option><option value=\"10\">10</option></select></div>");

    return false;
}

function editWeight() {
    var parentForm = $(this).parent();
    $(this).remove();
    parentForm.append("<input name=\"weight\" type=\"number\" autocomplete=\"off\" value=\"@user.CurrentWeight\" />");
    parentForm.append("<div class=\"register-field\"><button class=\"btn btn-info\" value=\"submit\" type=\"submit\">Update Today's Weight</button></div>)");
}
