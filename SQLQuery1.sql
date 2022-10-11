create database BankManagementSystem
use BankManagementSystem

create login Manager with password='Manager'

create login Employee with Password='Employee'
create user Employee_User for login Employee

create login Customer with Password='Customer'
create user Customer_User for login Customer
--Tables

create Table Employee
(
	EmployeeID int primary key identity(10,1),
	FirstName varchar(20),
	LastName varchar(20),
	DOB datetime,
	Phone varchar(20),
	WorkedHour int Default 0,
	Username varchar(20) unique,
	Password varchar(20),
	HiredBy int foreign key references Manager(ManagerID) default 1,
	Photo image
)
create Table CustomerAccount
(
	AccountNo int primary key identity(1000,1),
	FirstName varchar(20),
	LastName varchar(20),
	DOB datetime,
	Phone varchar(20),
	Balance money,
	Username varchar(20) unique,
	Password varchar(20),
	CreatedBy varchar(20) foreign key references Employee(Username),
	Photo image
)
create Table Manager
(
	ManagerID int primary key identity(1,1),
	FirstName varchar(20),
	LastName varchar(20),
)
insert into Manager values ('Abiy','Abiy')

create Table Loan
(
	LoanID int primary key identity(100,1),
	FirstName varchar(20),
	LastName varchar(20),
	LoanAmount money,
	RepaymentDate datetime,
	AccountNo int foreign key references CustomerAccount(AccountNo),
	Status varchar(20) default 'Pending',
)
create Table TransactionLog
(
	TransactionID int primary key identity(500,1),
	Type varchar(20),
	Amount money,
	Date datetime,
	AffectedAccount int foreign key references CustomerAccount(AccountNo),
)
create table Messege
(
 MessegeID int primary key identity(1,1),
 Messege varchar(max),
 AccountNo int foreign key references CustomerAccount(AccountNo)
)



--procedure

create proc sp_InsertIntoEmployee
@FirstName varchar(20),@LastName varchar(20),@DOB datetime,@Phone varchar(20),
@Username varchar(20),@Password varchar(20),@Photo image
as 
begin
insert into Employee 
   (FirstName,LastName,DOB,Phone,Username,Password,Photo)
    values(@FirstName,@LastName,@DOB,@Phone,@Username,@Password,@Photo)
end

create proc sp_SearchEmployee
@Target varchar(20)
as 
begin
	select *from Employee where FirstName like upper(Left(@Target,1))+'%'
end

alter proc sp_UpdateEmployee
@EmployeeID int,@FirstName varchar(20),@LastName varchar(20),@DOB datetime,@Phone varchar(20),
@Username varchar(20),@Password varchar(20),@Photo image
as 
begin
	Update Employee
	set FirstName=dbo.FormatGivenName(@FirstName),LastName=dbo.FormatGivenName(@LastName),
		DOB=@DOB,Phone=@Phone,Username=@Username,Password=@Password,Photo=@Photo 
		where EmployeeID=@EmployeeID
end

create proc sp_DeleteEmployee
@EmployeeID int
as 
begin
	delete from Employee where EmployeeID =@EmployeeID
end

alter proc sp_DisplayEmployee
as 
begin
	select *from Employee 
end

create proc sp_VerifyEmployee
@Username varchar(30),@Password varchar(30)
as
begin
	if exists (select * from Employee where Username=@Username and Password=@Password)
		select 'Successful'
	else
		select 'UnSuccessful'
end

create proc sp_VerifyCustomer
@Username varchar(30),@Password varchar(30)
as
begin
	if exists (select * from CustomerAccount where Username=@Username and Password=@Password)
		select 'Successful'
	else
		select 'UnSuccessful'
end



create proc sp_InsertIntoCustomerAccount
@FirstName varchar(20),@LastName varchar(20),@DOB datetime,@Phone varchar(20),
@Balance money,@Username varchar(20),@Password varchar(20),@CreatedBy varchar(20),@Photo image
as 
begin
insert into CustomerAccount
   (FirstName,LastName,DOB,Phone,Balance,Username,Password,CreatedBy,Photo)
    values(@FirstName,@LastName,@DOB,@Phone,@Balance,@Username,@Password,@CreatedBy,@Photo)
end

create proc sp_SearchCustomer
@Target varchar(20)
as 
begin
	select *from CustomerAccount where FirstName like upper(Left(@Target,1))+'%'
end

create proc sp_UpdateCustomerAccount
@AccountNo int,@FirstName varchar(20),@LastName varchar(20),@DOB datetime,@Phone varchar(20),
@Balance money,@Username varchar(20),@Password varchar(20),@Photo image
as 
begin
	Update CustomerAccount
	set FirstName=dbo.FormatGivenName(@FirstName),LastName=dbo.FormatGivenName(@LastName),
		DOB=@DOB,Phone=@Phone,Balance=@Balance,Username=@Username,Password=@Password,Photo=@Photo 
		where AccountNo=@AccountNo
end

create proc sp_DeleteCustomer
@AccountNo int
as 
begin
	delete from CustomerAccount where AccountNo=@AccountNo
end

alter proc sp_DepositMoney
@To int,@Amount money
as
begin
	if(exists (select AccountNo from CustomerAccount where AccountNo=@To))
	begin
	Update CustomerAccount set Balance=Balance+@Amount
						     where AccountNo=@To
	select 'Successful'
	end
	else
		select 'Unsuccessful'
end

alter proc sp_WithdrawMoney
@From int,@Amount money
as
begin
	declare @currentBalance money,@status varchar(15);
	select @currentBalance=Balance from CustomerAccount where AccountNo=@From
	set @status=dbo.checkBalance(@Amount,@currentBalance)
	if(@status ='Sufficient')
	begin
		if(exists (select AccountNo from CustomerAccount where AccountNo=@From))
		begin
			Update CustomerAccount set Balance=Balance- @Amount
								 where AccountNo=@From
			if(@@ROWCOUNT > 0)
				select 'Successful'
		end
	end
	else
		select 'Unsuccessful'
end

alter proc sp_TransferMoney
@From int,@To int,@Amount money
as
begin
	declare @currentBalance money,@status varchar(15);
	select @currentBalance=Balance from CustomerAccount where AccountNo=@From
	set @status=dbo.checkBalance(@Amount,@currentBalance)
	if(@status ='Sufficient' )
	begin
		begin transaction TransferMoney
		if(exists(select AccountNo from CustomerAccount where AccountNo=@From))
		begin
			Update CustomerAccount set Balance=Balance - @Amount
								 where AccountNo=@From
		end
		if(@@ROWCOUNT = 0)
			select 'Unsuccessful'
		else
		begin
			if(exists(select AccountNo from CustomerAccount where AccountNo=@To))
			begin
			Update CustomerAccount set Balance=Balance + @Amount
								   where AccountNo=@To
			end
			if(@@ROWCOUNT = 0)
			begin
				rollback transaction
				select 'Unsuccessful'
			end
			else
				commit transaction
				select 'Successful'
		end
	end
end 

create proc sp_GetCustomerInfo
@Username varchar(30)
as
begin
	select *from CustomerAccount where Username=@Username
end

alter proc sp_GetMessege
@Username varchar(30)
as
begin
	declare @AccountNo int 
	select @AccountNo=AccountNo from CustomerAccount where Username=@Username
	select Messege from Messege where AccountNo=@AccountNo
end
select *from Messege


alter proc sp_InsertIntoLoan
@FirstName varchar(30),@LastName varchar(30),@Amount money,
@RepaymentDate datetime,@AccountNo int,@Status varchar(20)
as
begin
	if (exists(select AccountNo from Loan where AccountNo=@AccountNo))
		select 'Unsuccessful'
	else
	begin
		insert into Loan values(dbo.formatGivenName(@FirstName),dbo.formatGivenName(@LastName),
								  @Amount,@RepaymentDate,@AccountNo,@Status)
		select 'Successful'
	end
end

create proc sp_GetPendingLoans
as 
begin
	select *from Loan where Status='Pending'
end

create proc sp_ApproveLoan
@LoanID int
as 
begin
	update Loan set Status='Approved'
	where LoanID=@LoanID
end

create proc sp_DenyLoan
@LoanID int
as 
begin
	update Loan set Status='Denied'
	where LoanID=@LoanID
end

create proc sp_GetDeptors
as 
begin
	select * from GetDeptors()
end
create proc sp_DisplayTransactions
as 
begin
	select * from DisplayTransactions()
end


--functions

create function GetDeptors()
returns table
as
	return (select c.AccountNo,c.FirstName,c.LastName,l.LoanAmount from CustomerAccount c
			join Loan l on l.AccountNo=c.AccountNo )

alter function DisplayTransactions()
returns table
as
	return (select t.Type,t.Date as TransactionDate,c.FirstName+' '+c.LastName as FullName from TransactionLog t join CustomerAccount c
			on c.AccountNo=t.AffectedAccount)


create function checkBalance(@Amount money,@CurrentBalance money)
returns varchar(15)
as
begin
	declare @status varchar(15)
	if(@Amount > @CurrentBalance)
		set @status='Insufficient'
	else
		set @status='Sufficient'
	return @status
end


create function FormatGivenName(@Name varchar(30))
returns varchar(50)
as
begin
	return TRIM(UPPER(@Name))
end

create function GetTransactionInfo(@previousAmount money,@currentAmount money)
returns @transactionInfo table( Type varchar(20),TransactionAmount money)
as
begin
	declare @type varchar(20),@transactionAmount money
	if(@previousAmount <= @currentAmount)
		set @type='Deposit'
	else
		set @type='Withdraw'

	set @transactionAmount=@currentAmount-@previousAmount

	insert @transactionInfo
	values(@type,@transactionAmount)
	return
end


--trigger
create trigger UpdateBalance
on Loan
after update
as
begin
	if(update(Status))
	begin
		declare @LoanID int,@Status varchar(20),@AccountNo int,@Amount money
		select @LoanID=LoanID from inserted
		select @Status=Status from inserted
		select @Amount=LoanAmount from inserted
		select @AccountNo=AccountNo from inserted
		if(@Status= 'Approved')
		begin
			update CustomerAccount set Balance +=@Amount
			where AccountNo=@AccountNo
		end
		if(@Status= 'Denied')
		begin
			delete from Loan where LoanID=@LoanID
		end
	end
end

create trigger trg_RecordTransaction
on CustomerAccount
after update
as
begin
	if(update(Balance))
	begin
		declare @previousAmount money,@currentAmount money,@AffectedAccount int
		select @previousAmount=Balance from deleted
		select @currentAmount=Balance from inserted
		select @AffectedAccount=AccountNo from inserted
		insert into TransactionLog 
		select Type,TransactionAmount,getdate(),@AffectedAccount 
		from GetTransactionInfo(@previousAmount,@currentAmount)
	end		
end

alter trigger trg_WriteMessege
on CustomerAccount
after update
as
begin
	if(update(Balance))
	begin
		declare @previousAmount money,@currentAmount money,@AffectedAccount int
		select @previousAmount=Balance from deleted
		select @currentAmount=Balance from inserted
		select @AffectedAccount=AccountNo from inserted
		insert into Messege
		select cast(TransactionAmount as varchar(30))+'Birr Have been '+Type+' To your Account ',@AffectedAccount
		from GetTransactionInfo(@previousAmount,@currentAmount)
	end		
end


alter trigger trg_WriteUpdateMessege
on CustomerAccount
after update
as
begin
	if(not(update(Balance)))
	begin
		declare @AffectedAccount int
		select @AffectedAccount=AccountNo from inserted
		insert into Messege
		select 'Your Account Have Updated',@AffectedAccount
	end
end

create trigger trg_WriteLoanStatusMessege
on Loan
after update
as
begin
	if(update(Status))
	begin
		declare @AffectedAccount int,@Status varchar(20)
		select @AffectedAccount=AccountNo from inserted
		select @Status=Status from inserted
		if(@Status = 'Approved')
		begin
			insert into Messege
			select 'Your Loan Request have been Approved',@AffectedAccount
		end
		else
		begin
			insert into Messege
			select 'Your Loan Request have been Denied',@AffectedAccount
		end
	end
end





select *from Loan
select *from TransactionLog