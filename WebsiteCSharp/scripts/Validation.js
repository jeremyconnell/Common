//PURPOSE: Force Upper Case Letters
//USAGE: onkeyup="return ForceUpperCase()" 
function ForceUpperCase(txt)
{ txt.value = txt.value.toUpperCase(); }


//PURPOSE: Prevent alphanumeric chars being typed in (including fullstop)
//USAGE: onKeyPress="return ValidateInteger()" 
function ValidateInteger() {
    var ASCII = event.keyCode;
    if (IsNormalInput(ASCII)) { return true; } //TAB etc
    if (ASCII == 45) { return true; } //189=neg. sign
    if ((ASCII >= 48) && (ASCII <= 57)) { return true; } //(0-9)=(48-57)
    return false;
}

//PURPOSE: Prevent alphanumeric chars being typed in (excluding fullstop)
//USAGE: onKeyPress="return ValidateNumber()" 
function ValidateNumber() {
    var ASCII = event.keyCode;
    if (ValidateInteger(ASCII)) { return true; }
    if (ASCII == 46) { return true; } //46=dec. point
    return false;
}
//PURPOSE: Money format, Prevent alphanumeric chars being typed in (excluding fullstop)
//USAGE: onKeyPress="return ValidateMoney()"
function ValidateMoney() {
    var ASCII = event.keyCode;
    if (ValidateNumber(ASCII)) { return true; }
    if (ASCII == 36) { return true; } //36=$ sign
    return false;
}
//PURPOSE: Percentage format, Prevent alphanumeric chars being typed in (excluding fullstop)
//USAGE: onKeyPress="return ValidatePercentage()"
function ValidatePercentage() {
    var ASCII = event.keyCode;
    if (ValidateNumber(ASCII)) { return true; }
    if (ASCII == 36) { return true; } //37=% sign
    return false;
} 
//PURPOSE: Allow up/down arrow keys to increment a number field
//USAGE: onKeyDown="IncrementOrDecrementNumber(this)"
function IncrementOrDecrementNumber(txt) {
    var s = txt.value.replace('$', '');
    var d = parseFloat(s);
    if (isNaN(d)) return false;
    if (38 == event.keyCode) { try { txt.value = d + 1; txt.select(); } catch (e) { ; } }
    if (40 == event.keyCode) { try { txt.value = d - 1; txt.select(); } catch (e) { ; } }
    return true;
}

//PURPOSE: Force a number to be greater than zero
//USAGE: onBlur="BlurWholeNumber(this)"
function BlurWholeNumber(txt) {
    try { if (txt.value == '') { txt.value = 0; } } catch (e) { ; }
    try { if (parseInt(txt.value) < 0) { txt.value = 0; } } catch (e) { ; }
}

//PURPOSE: Force a number into currency format
//USAGE: onBlur="BlurMoney(this)"
function BlurMoney(txt) {
    txt.value = CurrencyFormatted(txt.value);
}
function BlurNumber(txt) {
    txt.value = ('' + txt.value).replace(/[a-z]/gi, '');
}

function IsNormalInput(keyCode) {
    //debug only: window.status += ("," + keyCode);
    if (keyCode == 9) return true; //Allow TAB
    if (keyCode == 17) return true; //Allow CTRL
    if (keyCode == 67) return true; //Allow C
    if (keyCode == 86) return true; //Allow V

    if (keyCode >= 48 && keyCode <= 57) return true;  //Allow numpad numbers
    if (keyCode == 46 || keyCode == 45) return true; //Allow numpad dot and minus

    if (keyCode >= 96 && keyCode <= 105) return true;  //Allow numpad numbers
    if (keyCode == 109 || keyCode == 110) return true; //Allow numpad dot and minus
    return false;
}
function CurrencyFormatted(amount) {
    amount = ('' + amount).replace(/[a-z$,]/gi, '');
    var i = parseFloat(amount);
    if (isNaN(i)) { return '$'; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    i = parseInt((i + .005) * 100);
    i = i / 100;
    s = new String(i);
    if (s.indexOf('.') < 0) { s += '.00'; }
    if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
    if (s.length > 6) { s = s.substring(0, s.length - 6) + ',' + s.substring(s.length - 6, s.length); }
    if (s.length > 10) { s = s.substring(0, s.length - 10) + ',' + s.substring(s.length - 10, s.length); }
    if (s.length > 14) { s = s.substring(0, s.length - 14) + ',' + s.substring(s.length - 14, s.length); }
    if (s.length > 18) { s = s.substring(0, s.length - 18) + ',' + s.substring(s.length - 18, s.length); }
    s = minus + s;
    return '$' + s;
}

