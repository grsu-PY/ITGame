/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     29.11.2014 21:06:15                          */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('Armor')
            and   type = 'U')
   drop table Armor
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Humanoid')
            and   type = 'U')
   drop table Humanoid
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Humanoid_Armor')
            and   name  = 'HUMANOID_ARMOR_FK'
            and   indid > 0
            and   indid < 255)
   drop index Humanoid_Armor.HUMANOID_ARMOR_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Humanoid_Armor')
            and   type = 'U')
   drop table Humanoid_Armor
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Humanoid_Spell')
            and   name  = 'HUMANOID_SPELL_FK'
            and   indid > 0
            and   indid < 255)
   drop index Humanoid_Spell.HUMANOID_SPELL_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Humanoid_Spell')
            and   type = 'U')
   drop table Humanoid_Spell
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Humanoid_Weapon')
            and   name  = 'HUMANOID_WEAPON_FK'
            and   indid > 0
            and   indid < 255)
   drop index Humanoid_Weapon.HUMANOID_WEAPON_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Humanoid_Weapon')
            and   type = 'U')
   drop table Humanoid_Weapon
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Spell')
            and   type = 'U')
   drop table Spell
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Surface')
            and   type = 'U')
   drop table Surface
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SurfaceRule')
            and   type = 'U')
   drop table SurfaceRule
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Weapon')
            and   type = 'U')
   drop table Weapon
go

/*==============================================================*/
/* Table: Armor                                                 */
/*==============================================================*/
create table Armor (
   Id                   uniqueidentifier     not null,
   ArmorType            varchar(20)          not null,
   PhysicalDef          int                  not null,
   MagicalDef           int                  not null,
   Name                 varchar(40)          not null,
   constraint PK_ARMOR primary key (Id)
)
go

/*==============================================================*/
/* Table: Humanoid                                              */
/*==============================================================*/
create table Humanoid (
   Id                   uniqueidentifier     not null,
   HumanoidRace         varchar(20)          not null,
   Weapon               varchar(36)          null,
   AttackSpell          varchar(36)          null,
   DefensiveSpell       varchar(36)          null,
   HP                   int                  not null,
   MP                   int                  not null,
   Strength             int                  not null,
   Agility              int                  not null,
   Wisdom               int                  not null,
   Constitution         int                  not null,
   Name                 varchar(40)          not null,
   constraint PK_HUMANOID primary key (Id)
)
go

/*==============================================================*/
/* Table: Humanoid_Armor                                        */
/*==============================================================*/
create table Humanoid_Armor (
   Humanoid_Id          uniqueidentifier     not null,
   Armor_Id             uniqueidentifier     not null,
   constraint PK_HUMANOID_ARMOR primary key (Humanoid_Id, Armor_Id)
)
go

/*==============================================================*/
/* Index: HUMANOID_ARMOR_FK                                     */
/*==============================================================*/
create index HUMANOID_ARMOR_FK on Humanoid_Armor (
Humanoid_Id ASC
)
go

/*==============================================================*/
/* Table: Humanoid_Spell                                        */
/*==============================================================*/
create table Humanoid_Spell (
   Humanoid_Id          uniqueidentifier     not null,
   Spell_Id             uniqueidentifier     not null,
   constraint PK_HUMANOID_SPELL primary key (Humanoid_Id, Spell_Id)
)
go

/*==============================================================*/
/* Index: HUMANOID_SPELL_FK                                     */
/*==============================================================*/
create index HUMANOID_SPELL_FK on Humanoid_Spell (
Humanoid_Id ASC
)
go

/*==============================================================*/
/* Table: Humanoid_Weapon                                       */
/*==============================================================*/
create table Humanoid_Weapon (
   Humanoid_Id          uniqueidentifier     not null,
   Weapon_Id            uniqueidentifier     not null,
   constraint PK_HUMANOID_WEAPON primary key (Humanoid_Id, Weapon_Id)
)
go

/*==============================================================*/
/* Index: HUMANOID_WEAPON_FK                                    */
/*==============================================================*/
create index HUMANOID_WEAPON_FK on Humanoid_Weapon (
Humanoid_Id ASC
)
go

/*==============================================================*/
/* Table: Spell                                                 */
/*==============================================================*/
create table Spell (
   Id                   uniqueidentifier     not null,
   SpellType            varchar(20)          not null,
   SchoolSpell          varchar(20)          not null,
   MagicalPower         int                  not null,
   ManaCost             int                  not null,
   TotalDuration        int                  not null,
   Name                 varchar(40)          not null,
   constraint PK_SPELL primary key (Id)
)
go

/*==============================================================*/
/* Table: Surface                                               */
/*==============================================================*/
create table Surface (
   CurrentSurfaceType   varchar(40)          not null,
   HumanoidRace         varchar(20)          not null,
   constraint PK_SURFACE primary key nonclustered (CurrentSurfaceType)
)
go

/*==============================================================*/
/* Table: SurfaceRule                                           */
/*==============================================================*/
create table SurfaceRule (
   CurrentSurfaceType   varchar(40)          not null,
   HumanoidRace         varchar(20)          not null,
   HP                   int                  not null,
   MP                   int                  not null,
   Strength             int                  not null,
   Wisdom               int                  not null,
   Agility              int                  not null,
   Constitution         int                  not null,
   constraint PK_SURFACERULE primary key (CurrentSurfaceType)
)
go

/*==============================================================*/
/* Table: Weapon                                                */
/*==============================================================*/
create table Weapon (
   Id                   uniqueidentifier     not null,
   WeaponType           varchar(20)          not null,
   PhysicalAttack       int                  not null,
   MagicalAttack        int                  not null,
   Name                 varchar(40)          not null,
   constraint PK_WEAPON primary key (Id)
)
go

