<?php

	//	* Script by Praveen
	//	* http://blog.ninethsense.com
		
    // A very basic PHP example for the sake of showing server side code
    // Code is Vulnerable. Please apply security thoughts

    foreach ($_FILES['file']['tmp_name'] as $k => $v) {
        move_uploaded_file(
                            $_FILES['file']['tmp_name'][$k], 
                            $_FILES['file']['name'][$k]
                    );
    }
	echo "Success"; // Fake success value
?>


