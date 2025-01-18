--database service_center

create type order_status as enum ('in progress', 'waiting for payment', 'completed')

drop type if exists order_status cascade 

create domain person_name as
	varchar(50) not null check (length(value) > 1)
	
drop domain if exists person_name cascade
	
create domain phone_number as
	varchar(13) not null check (value ~ '^\+380[0-9]{9}$')
	
drop domain if exists phone_number cascade
	
create domain money_amount as
	decimal(10, 2) not null check (value > 0)
	
drop domain if exists money_amount cascade

create table if not exists customers (
	customer_id int primary key generated always as identity,
  last_name person_name,
	first_name person_name,
	middle_name person_name,
  full_name varchar generated always as (last_name || ' ' || first_name || ' ' || middle_name) stored,
	phone_number phone_number unique,
	email varchar(320) null unique
)

drop table if exists customers

create table if not exists employees (
	employee_id int primary key generated always as identity,
	last_name person_name,
	first_name person_name,
	middle_name person_name,
  full_name varchar generated always as (last_name || ' ' || first_name || ' ' || middle_name) stored,
	phone_number phone_number unique,
	email varchar(320) not null unique,
	specialization text not null
)

drop table if exists employees 

create table if not exists orders (
	order_id int primary key generated always as identity,
	customer_id int not null,
	service_type_id int not null,
	employee_id int not null,
	description text not null,
	price money_amount not null,
	status order_status not null default 'in progress',
	placed_at date not null check(placed_at <= current_date) default current_date,
	due_by date not null check(due_by >= placed_at) default current_date,
	updated_at timestamp not null default current_timestamp,
	constraint fk_customer_id
		foreign key (customer_id)
		references customers (customer_id)
		on delete restrict,
	constraint fk_service_type_id
		foreign key (service_type_id)
		references service_types (service_type_id)
		on delete restrict,
	constraint fk_employee_id
		foreign key (employee_id)
		references employees (employee_id)
		on delete restrict
)

drop table if exists orders

create trigger log_order_update_timestamp
after update on orders
for each row
execute function log_update_timestamp();

create trigger validate_order_insertion 
before insert on orders
for each row 
execute function prevent_certain_order_insertion();

create trigger validate_order_price
before insert or update on orders
for each row
execute function validate_order_price();

create trigger validate_order_before_update
before update on orders
for each row
execute function validate_order_before_update();

create table if not exists service_types (
	service_type_id int primary key generated always as identity,
	name varchar(100) not null unique,
	description text null,
	min_price money_amount not null,
	estimated_time interval null
)

drop table if exists service_types

create table if not exists payments (
	payment_id int primary key generated always as identity,
	order_id int not null,
	amount money_amount not null,
	constraint fk_order_id
		foreign key (order_id)
		references orders (order_id)
		on delete cascade
)

drop table if exists payments

create trigger automated_order_status_change
after insert or update on payments
for each row
execute function change_order_status_if_needed();

create or replace function log_update_timestamp() returns trigger as 
$$
begin
  new.updated_at = current_timestamp;
  return new;
end
$$ language plpgsql;

create or replace function prevent_certain_order_insertion() returns trigger as 
$$ 
begin
	if new.status = 'completed' then
		raise exception 'Can''t insert an already completed order!';
	end if;

	return new;
end
$$ language plpgsql;

create or replace function validate_order_price() returns trigger as
$$
begin
	if new.price < (select st.min_price from service_types st where st.service_type_id = new.service_type_id) then
		raise exception 'Order''s price can''t be less than service type''s minimal price!';
	end if;

	return new;
end
$$ language plpgsql;

create or replace function validate_order_before_update() returns trigger as
$$ 
begin
	if old.status = 'completed'
		and old.price != new.price then
		raise exception 'Can''t update price of a completed order!';
	end if;

	if old.status = 'waiting for payment' 
		and new.status = 'completed' 
		and not order_has_been_paid_for(new.order_id) then
		raise exception 'This order hasn''t been paid for yet!';
	end if;

	if old.status = 'in progress'
		and new.status = 'waiting for payment'
		and order_has_been_paid_for(new.order_id) then
		raise exception 'This order has already been paid for, so change the status to ''completed''!';
	end if;
	
	return new;
end
$$ language plpgsql;

create or replace function change_order_status_if_needed() returns trigger as
$$
begin
	if order_has_been_paid_for(new.order_id) then
		update orders
    set status = 'completed'
    where order_id = new.order_id and status = 'waiting for payment';
	end if;

	return new;
end
$$ language plpgsql;

create or replace function get_total_payment(func_order_id int)
returns money_amount
language plpgsql
as
$$
begin
	return coalesce(
  	(select sum(p.amount) from payments p where p.order_id = func_order_id), 
		0
	);
end
$$

create or replace function order_has_been_paid_for(func_order_id int)
returns boolean
language plpgsql
as
$$
begin
	return get_total_payment(func_order_id) >= (select price from orders o where o.order_id = func_order_id);
end
$$

select * from customers 

select * from employees

select * from orders

select * from service_types

select * from payments












