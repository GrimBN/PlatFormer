mergeInto(LibraryManager.library, {

  SaveToLocalStorage: function (str) {
    localStorage.setItem("saveData", UTF8ToString(str));
  },

  GetFromLocalStorage: function () {

    var returnStr = localStorage.getItem("saveData");
    if(!returnStr)
    {
        return null;
    }
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },
});