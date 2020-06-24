<html>
    <head>
        <meta http-equiv="content-type" content="text/html; charset=utf-8">
        <style> 
            * {
                font-family: Arial, Helvetica, sans-serif;
            }
        </style>
    </head>
<body>
    <?php
        $f = key($_GET);
        if (file_exists("$f.html")) {
            echo file_get_contents("$f.html");
        } else {
            echo "{Error in preview}";
        }
    ?>
</body>
</html>