/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     30.11.2014 19:40:48                          */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Hum_Armor') and o.name = 'FK_HUMANOID_ARMOR')
alter table Hum_Armor
   drop constraint FK_HUMANOID_ARMOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Hum_Armor') and o.name = 'FK_ARMOR_HUMANOID')
alter table Hum_Armor
   drop constraint FK_ARMOR_HUMANOID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Hum_Spell') and o.name = 'FK_SPELL_HUMANOID')
alter table Hum_Spell
   drop constraint FK_SPELL_HUMANOID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Hum_Spell') and o.name = 'FK_HUMANOID_SPELL')
alter table Hum_Spell
   drop constraint FK_HUMANOID_SPELL
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Hum_Weapon') and o.name = 'FK_WEAPON_HUMANOID')
alter table Hum_Weapon
   drop constraint FK_WEAPON_HUMANOID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Hum_Weapon') and o.name = 'FK_HUMANOID_WEAPON')
alter table Hum_Weapon
   drop constraint FK_HUMANOID_WEAPON
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Humanoid') and o.name = 'FK_HUMANOID_CHARACTER')
alter table Humanoid
   drop constraint FK_HUMANOID_CHARACTER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Humanoid') and o.name = 'FK_HUMANOID_HUMANOID_RACE')
alter table Humanoid
   drop constraint FK_HUMANOID_HUMANOID_RACE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SurfaceRule') and o.name = 'FK_SURFACER_SURFACERU_SURFACE')
alter table SurfaceRule
   drop constraint FK_SURFACER_SURFACERU_SURFACE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Armor')
            and   type = 'U')
   drop table Armor
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Character')
            and   name  = 'CHARACTER_INDEX_PK'
            and   indid > 0
            and   indid < 255)
   drop index Character.CHARACTER_INDEX_PK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Character')
            and   type = 'U')
   drop table Character
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Hum_Armor')
            and   name  = 'Armor_Id_Index'
            and   indid > 0
            and   indid < 255)
   drop index Hum_Armor.Armor_Id_Index
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Hum_Armor')
            and   name  = 'Hum_Id_Index'
            and   indid > 0
            and   indid < 255)
   drop index Hum_Armor.Hum_Id_Index
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Hum_Armor')
            and   type = 'U')
   drop table Hum_Armor
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Hum_Spell')
            and   name  = 'Spell_Id_Index'
            and   indid > 0
            and   indid < 255)
   drop index Hum_Spell.Spell_Id_Index
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Hum_Spell')
            and   name  = 'Hum_Id_Index'
            and   indid > 0
            and   indid < 255)
   drop index Hum_Spell.Hum_Id_Index
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Hum_Spell')
            and   type = 'U')
   drop table Hum_Spell
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Hum_Weapon')
            and   name  = 'Weap_Id_Index'
            and   indid > 0
            and   indid < 255)
   drop index Hum_Weapon.Weap_Id_Index
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Hum_Weapon')
            and   name  = 'Hum_Id_Index'
            and   indid > 0
            and   indid < 255)
   drop index Hum_Weapon.Hum_Id_Index
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Hum_Weapon')
            and   type = 'U')
   drop table Hum_Weapon
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Humanoid')
            and   type = 'U')
   drop table Humanoid
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HumanoidRace')
            and   name  = 'HUMRACE_INDEX_PK'
            and   indid > 0
            and   indid < 255)
   drop index HumanoidRace.HUMRACE_INDEX_PK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('HumanoidRace')
            and   type = 'U')
   drop table HumanoidRace
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
   ArmorType            tinyint              not null,
   PhysicalDef          int                  not null,
   MagicalDef           int                  not null,
   Name                 varchar(40)          not null,
   Weight               int                  not null,
   Equipped             bit                  not null,
   constraint PK_ARMOR primary key (Id)
)
go

/*==============================================================*/
/* Table: Character                                             */
/*==============================================================*/
create table Character (
   Id                   uniqueidentifier     not null,
   Password             varchar(Max)         not null,
   Name                 varchar(40)          not null,
   Role                 tinyint              not null,
   constraint PK_CHARACTER primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Index: CHARACTER_INDEX_PK                                    */
/*==============================================================*/
create unique clustered index CHARACTER_INDEX_PK on Character (
Id ASC
)
go

/*==============================================================*/
/* Table: Hum_Armor                                             */
/*==============================================================*/
create table Hum_Armor (
   Humanoid_Id          uniqueidentifier     not null,
   Armor_Id             uniqueidentifier     not null,
   constraint PK_HUM_ARMOR primary key nonclustered (Humanoid_Id, Armor_Id)
)
go

/*==============================================================*/
/* Index: Hum_Id_Index                                          */
/*==============================================================*/
create index Hum_Id_Index on Hum_Armor (
Humanoid_Id ASC
)
go

/*==============================================================*/
/* Index: Armor_Id_Index                                        */
/*==============================================================*/
create index Armor_Id_Index on Hum_Armor (
Armor_Id ASC
)
go

/*==============================================================*/
/* Table: Hum_Spell                                             */
/*==============================================================*/
create table Hum_Spell (
   Humanoid_Id          uniqueidentifier     not null,
   Spell_Id             uniqueidentifier     not null,
   constraint PK_HUM_SPELL primary key nonclustered (Humanoid_Id, Spell_Id)
)
go

/*==============================================================*/
/* Index: Hum_Id_Index                                          */
/*==============================================================*/
create index Hum_Id_Index on Hum_Spell (
Humanoid_Id ASC
)
go

/*==============================================================*/
/* Index: Spell_Id_Index                                        */
/*==============================================================*/
create index Spell_Id_Index on Hum_Spell (
Spell_Id ASC
)
go

/*==============================================================*/
/* Table: Hum_Weapon                                            */
/*==============================================================*/
create table Hum_Weapon (
   Humanoid_Id          uniqueidentifier     not null,
   Weapon_Id            uniqueidentifier     not null,
   constraint PK_HUM_WEAPON primary key nonclustered (Humanoid_Id, Weapon_Id)
)
go

/*==============================================================*/
/* Index: Hum_Id_Index                                          */
/*==============================================================*/
create index Hum_Id_Index on Hum_Weapon (
Humanoid_Id ASC
)
go

/*==============================================================*/
/* Index: Weap_Id_Index                                         */
/*==============================================================*/
create index Weap_Id_Index on Hum_Weapon (
Weapon_Id ASC
)
go

/*==============================================================*/
/* Table: Humanoid                                              */
/*==============================================================*/
create table Humanoid (
   Id                   uniqueidentifier     not null,
   HumanoidRaceType     tinyint              not null,
   HP                   int                  not null,
   MP                   int                  not null,
   Strength             int                  not null,
   Agility              int                  not null,
   Wisdom               int                  not null,
   Constitution         int                  not null,
   Name                 varchar(40)          not null,
   Level                tinyint              not null default 1
      constraint CKC_LEVEL_HUMANOID check (Level >= 1),
   constraint PK_HUMANOID primary key (Id)
)
go

/*==============================================================*/
/* Table: HumanoidRace                                          */
/*==============================================================*/
create table HumanoidRace (
   HumanoidRaceType     tinyint              not null,
   Strength             int                  not null,
   Agility              int                  not null,
   Wisdom               int                  not null,
   Constitution         int                  not null,
   Name                 varchar(40)          not null,
   constraint PK_HUMANOIDRACE primary key nonclustered (HumanoidRaceType)
)
go

/*==============================================================*/
/* Index: HUMRACE_INDEX_PK                                      */
/*==============================================================*/
create unique clustered index HUMRACE_INDEX_PK on HumanoidRace (
HumanoidRaceType ASC
)
go

/*==============================================================*/
/* Table: Spell                                                 */
/*==============================================================*/
create table Spell (
   Id                   uniqueidentifier     not null,
   SpellType            tinyint              not null,
   SchoolSpell          tinyint              not null,
   MagicalPower         int                  not null,
   ManaCost             int                  not null,
   TotalDuration        int                  not null,
   Name                 varchar(40)          not null,
   Equipped             bit                  not null,
   constraint PK_SPELL primary key (Id)
)
go

/*==============================================================*/
/* Table: Surface                                               */
/*==============================================================*/
create table Surface (
   CurrentSurfaceType   tinyint              not null,
   HumanoidRaceType     tinyint              not null,
   constraint PK_SURFACE primary key nonclustered (CurrentSurfaceType)
)
go

/*==============================================================*/
/* Table: SurfaceRule                                           */
/*==============================================================*/
create table SurfaceRule (
   CurrentSurfaceType   tinyint              not null,
   HumanoidRaceType     tinyint              not null,
   HP                   int                  not null,
   MP                   int                  not null,
   Strength             int                  not null,
   Agility              int                  not null,
   Wisdom               int                  not null,
   Constitution         int                  not null,
   constraint PK_SURFACERULE primary key (CurrentSurfaceType)
)
go

/*==============================================================*/
/* Table: Weapon                                                */
/*==============================================================*/
create table Weapon (
   Id                   uniqueidentifier     not null,
   WeaponType           tinyint              not null,
   PhysicalAttack       int                  not null,
   MagicalAttack        int                  not null,
   Name                 varchar(40)          not null,
   Weight               int                  not null,
   Equipped             bit                  not null,
   constraint PK_WEAPON primary key (Id)
)
go

alter table Hum_Armor
   add constraint FK_HUMANOID_ARMOR foreign key (Armor_Id)
      references Armor (Id)
         on update cascade on delete cascade
go

alter table Hum_Armor
   add constraint FK_ARMOR_HUMANOID foreign key (Humanoid_Id)
      references Humanoid (Id)
         on update cascade on delete cascade
go

alter table Hum_Spell
   add constraint FK_SPELL_HUMANOID foreign key (Humanoid_Id)
      references Humanoid (Id)
         on update cascade on delete cascade
go

alter table Hum_Spell
   add constraint FK_HUMANOID_SPELL foreign key (Spell_Id)
      references Spell (Id)
         on update cascade on delete cascade
go

alter table Hum_Weapon
   add constraint FK_WEAPON_HUMANOID foreign key (Humanoid_Id)
      references Humanoid (Id)
         on update cascade on delete cascade
go

alter table Hum_Weapon
   add constraint FK_HUMANOID_WEAPON foreign key (Weapon_Id)
      references Weapon (Id)
         on update cascade on delete cascade
go

alter table Humanoid
   add constraint FK_HUMANOID_CHARACTER foreign key (Id)
      references Character (Id)
go

alter table Humanoid
   add constraint FK_HUMANOID_HUMANOID_RACE foreign key (HumanoidRaceType)
      references HumanoidRace (HumanoidRaceType)
go

alter table SurfaceRule
   add constraint FK_SURFACER_SURFACERU_SURFACE foreign key (CurrentSurfaceType)
      references Surface (CurrentSurfaceType)
go

