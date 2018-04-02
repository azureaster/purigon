<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userName = $_POST["userNamePost"];


	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	//else echo("Connection Success");
	
	$sql = "SELECT * FROM userinfo WHERE NAME ='".$userName."'";
	$result = mysqli_query($conn, $sql);	
	//$count = mysqli_num_rows($result);

	if(mysqli_num_rows($result) == 0){
		echo "VALID";	
	}	
	else echo "INVALID";
	
	
	
	
	

?>