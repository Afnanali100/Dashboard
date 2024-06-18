CREATE TABLE Posts (
    post_id INT PRIMARY KEY identity(1,1),
    user_id nvarchar(450),
    description nvarchar(500),
	image_name nvarchar(max),
	video_name nvarchar(max),
    creation_date DATETIME default current_timestamp,
    FOREIGN KEY (user_id) REFERENCES AspNetUsers(Id)
);


CREATE TABLE Comments (
    comment_id INT PRIMARY KEY identity(1,1),
    post_id INT,
    user_id nvarchar(450),
    content nvarchar(500),
    creation_date DATETIME default current_timestamp,
    FOREIGN KEY (post_id) REFERENCES Posts(post_id),
    FOREIGN KEY (user_id) REFERENCES AspNetUsers(Id)
);

CREATE TABLE Likes (
    like_id INT PRIMARY KEY identity(1,1),
    post_id INT,
    user_id nvarchar(450),
    creation_date DATETIME default current_timestamp,
    FOREIGN KEY (post_id) REFERENCES Posts(post_id),
    FOREIGN KEY (user_id) REFERENCES AspNetUsers(Id)
);


alter table Posts
add  LikesCount int default 0
