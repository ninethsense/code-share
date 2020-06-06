<?php
	error_reporting(0);
	
	$host = $_GET["host"];
	$port = $_GET["port"];
	
	if (!isset($_GET["host"]) || !isset($_GET["port"]) ){
		die("Usage: /PortPing.php?host=target_name&port=port_number[&t=n]<br><br>Example: /PortPing.php?host=google.com&port=80");
	}
	$t =(isset($_GET["t"]))?$_GET["t"]:4; // default 4 pings.
	$ip = gethostbyname($host);
	
	if (isset($_GET["source"])) {
		die ( ping($host, $port) );
	}

	function ping($host, $port) {
		$startTime = time();
		if ($fs = fsockopen($host, $port, $errCode, $errStr, 1)) {
			$timeTaken = (time()-$startTime)/1000;
		echo "Reply from $host:$port time={$timeTaken}ms";
			fclose($fs);
		} else {	
			echo "Request timed out.";
		}
		echo "<br>";
	}
?>

<html>
	<head>
		<style>
			body {
				background-color:#000;
				color:#fff;
				font-family:"Courier New";
			}
			#console {
				margin:10 10;
			}
		</style>
	</head>
	<body>
		<div>Trying to connect <?php echo "$host:$port [$ip:$port]"; ?>:</div>
		
		<div id="console">
		</div>
		<script>
			var count = 0;
			var intervalID = setInterval(function PortPing() {
				var xhttp = new XMLHttpRequest();
				xhttp.onreadystatechange = function() {
					if (this.readyState == 4 && this.status == 200) {
						document.getElementById("console").innerHTML += this.responseText;
					}
				};
				xhttp.open("GET", "PortPing.php?source=self&host=<?=$host?>&port=<?=$port?>", true);
				xhttp.send();
				if (++count > <?=$t-1?>) {
					clearInterval(intervalID);
				}
			},2);
			
		</script>
	</body>
</html>
