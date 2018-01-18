
create table Customer
(
id nchar(9) primary key,
name nvarchar(10) not null,
pass char(9) not null,
isAdmin bit default(0) not null
);
---------------------------
create table Movie
(
id int identity(1,1) primary key,
name nvarchar(30) not null,
publish_date date default(getdate()),
langth smallint check(langth > 0) not null,
genre nvarchar(6) NOT NULL CHECK (genre IN('פעולה', 'מתח', 'קומדיה', 'דרמה', 'רומנטי', 'אימה')),
img nvarchar(100) default('first_img.JPG') not null
);
---------------------------
create table PlayTime
(
id int identity(1,1) primary key,
movie_id int foreign key references Movie(id),
play datetime2 not null,
total_sits int check(total_sits > 0) not null,
availble_sits int default(0) not null
);
------------------------
create table CustomerBuyTickets
(
id int identity(1,1) primary key,
customer_id nchar(9) foreign key references Customer(id),
playTime_id int foreign key references playTime(id),
amount int check(amount > 0)
);


--------------------
ALTER TABLE Movie ALTER COLUMN langth smallint not null

ALTER TABLE CustomerBuyTickets
  ADD playTime_id int foreign key references PlayTime(id)



---------------------
select *
from Customer

select *
from PlayTime

select *
from Movie

-----------





