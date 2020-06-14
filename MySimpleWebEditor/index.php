<?php
    /**
     *  A lightweight, single page HTML editor with live preview and auto-save
     *  Author: Praveen Nair @ http://blog.ninethsense.com/
     *  
     */

    if ( isset($_POST["c"]) )  {
        $content = $_POST["c"];
        echo  (file_put_contents("temp.txt", $content))?"1":"-1";
        die();
    }
?>
<html>
    <head>
        <meta http-equiv="content-type" content="text/html; charset=utf-8">
        <style>
            #tb {
                margin-left: 5;
            }
            #editor, #preview {
                flex: 1 0 0;
                background:#eee;
                margin: 5 5;
                padding: 5 5;
                overflow:auto;
            }
            #editor {
                font-family: monospace;
            }
            #preview {
                font-family: Arial, Helvetica, sans-serif;
            }
            [placeholder]:empty::before {
                content: attr(placeholder);
                color: #ccc; 
            }
            #saved {
                font-size:8pt;
                margin-left:5px;   
            }
        </style>
    </head>
    <body>
        <div id="tb">
            <input type="button" value="Save" onclick="Save()" /><span id="saved"></span>
        </div>
        <div style="display: flex;height:90%">
            
            <div id="editor" contenteditable="true" onkeyup="LivePreview()" placeholder="Your HTML Code"></div>

            <div id="preview" placeholder="Preview Area"></div>
        </div>

        <script>
            var editor = document.getElementById("editor");
            var preview = document.getElementById("preview");
            var IsSaved = true;
            editor.focus();
            function LivePreview() {
                preview.innerHTML = editor.innerText;
                IsSaved = false;
            }
            function Save() {
                var xhttp = new XMLHttpRequest();
                xhttp.onreadystatechange = function() {
                    if (this.readyState == 4 && this.status == 200) {
                        if (xhttp.responseText == "1") {
                            document.getElementById("saved").style.color = 'green';
                            document.getElementById("saved").innerText = "Last saved at " + new Date() ;
                            IsSaved = true;
                        } else {
                            ShowError();
                        }
                    } 
                };
                var formData = new FormData();
                formData.append("c", editor.innerText);
                xhttp.open("POST", "<?= $_SERVER["PHP_SELF"] ?>", true);
                xhttp.send(formData);

            }
            setInterval(() => {
                if (!IsSaved) {
                    Save();
                }
            }, 5000);

            ShowError = function() {
                var ele = document.getElementById("saved");
                ele.style.color = 'red';
                ele.innerText = "Couldn't Auto-save. Will retry soon" ;
            }
            window.onload = function() {
                <?php                
                    if ( $_SERVER['REQUEST_METHOD'] != 'POST' ) {
                        $content = file_get_contents("temp.txt");
                        echo "editor.innerText = `$content`;";
                    }
                ?>
                LivePreview();
            }
            
        </script>
    </body>
</html>
