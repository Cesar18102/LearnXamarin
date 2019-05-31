<?php
	if(isset($_POST['hash']) && isset($_POST['rand']) && $_POST['hash'] == md5("27052019".$_POST['rand']) &&
	   isset($_POST['query']) && $_POST['query'] != "") {
		
		include "DB_Request.php";
		$db_link = Connect();
		
		$resp = Request($db_link, $_POST['query']);
		
		$response = "[ ";
		
		while($cortage = mysqli_fetch_array($resp, MYSQLI_ASSOC)) {
            $object = "{ ";
			foreach($cortage as $key => $value) 
				$object .= '"'.$key.'" : '.(preg_match("/^\d+$/", $value) == 1 ? $value : '"'.$value.'"').', ';
			$object = ($object == "{ " ? "{ }" : substr($object, 0, strlen($object) - 2)." }");
            $response .= $object.", ";
        }
		
		
		echo $response == "[ " ? "[ { } ]" : substr($response, 0, strlen($response) - 2)." ]";
	}
    else
		echo "error";
?>					