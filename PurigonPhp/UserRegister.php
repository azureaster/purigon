<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userID = $_POST["userIDPost"];
	$userPW = $_POST["userPWPost"];
	$userEmail = $_POST["userEmailPost"];
	$userName = $_POST["userNamePost"];
	$userLV = 1;
	$userEXP = 1;
	$userChar = 1; 
	$userSkill = 1;


	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "SELECT * FROM userinfo WHERE ID ='".$userID."'";
	$result = mysqli_query($conn, $sql);
	$IDcount = mysqli_num_rows($result);

	if($userID == "" || $userName == ""){
		echo "You need to enter ID and NickName <br>";
	}else{
		if(($IDcount) == 0){
			$sql = "SELECT * FROM userinfo WHERE NAME ='".$userName."'";
			$result = mysqli_query($conn, $sql);
			
			$Namecount = mysqli_num_rows($Nameresult);
			
			if($Namecount == 0){
				$sql = "INSERT INTO userinfo(ID, PW, EMAIL, NAME, LV, EXP, CHARNUM, SKILLNUM)
						VALUES ('".$userID."','".$userPW."','".$userEmail."','".$userName."','".$userLV."','".$userEXP."','".$userChar."','".$userSkill."')";
				$result = mysqli_query($conn, $sql);
				
				if(!result) echo "Error occered while inserting data to DB";
				else echo "Register completed";
			}
			else echo "Already Existing NickName";
		}
		else echo "Already Existing ID";
	}

		
	
	
	
	
	

?>