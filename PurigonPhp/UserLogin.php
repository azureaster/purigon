<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userID = $_POST["userIDPost"];
	$userPW = $_POST["userPWPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "SELECT PW FROM userinfo WHERE ID = '".$userID."' ";
	$result = mysqli_query($conn, $sql);
	
	//Get the result and confirm login
	if(mysqli_num_rows($result) > 0){
		//show data for each row
		while($row = mysqli_fetch_assoc($result)){
			if($row['PW'] == $userPW){
				echo "login success";
			}else
				echo "password incorrect";
		}
	}else{
		echo "user not found";
	}

?>