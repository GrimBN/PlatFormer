mergeInto(LibraryManager.library, {

  SaveToLocalStorage: function (str) {
    localStorage.setItem("saveData", UTF8ToString(str));
  },

  GetFromLocalStorage: function () {

    var returnStr = localStorage.getItem("saveData");
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },
});