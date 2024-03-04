<?php
session_start();

if(!isset($_SESSION['c'])) {
	$_SESSION['c'] = file_get_contents("./counter.txt");
	file_put_contents("./counter.txt", ++$_SESSION['c']);
}
?>
<html>
<head>
	<title>I will Google before asking dumb questions</title>
</head>
<body style="background-image:url('google1.jpg');background-size:contain" >
	<span>Don't worry, you are the <?= $_SESSION['c'] ?><sup>th</sup> person.</span>
<body>
</html>