/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     11/29/2014 8:21:12 AM                        */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('ARMOR')
            and   type = 'U')
   drop table ARMOR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('HUMANOID')
            and   type = 'U')
   drop table HUMANOID
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HUMANOID_ARMOR')
            and   name  = 'HUMANOID_ARMOR2_FK'
            and   indid > 0
            and   indid < 255)
   drop index HUMANOID_ARMOR.HUMANOID_ARMOR2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HUMANOID_ARMOR')
            and   name  = 'HUMANOID_ARMOR_FK'
            and   indid > 0
            and   indid < 255)
   drop index HUMANOID_ARMOR.HUMANOID_ARMOR_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('HUMANOID_ARMOR')
            and   type = 'U')
   drop table HUMANOID_ARMOR
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HUMANOID_SPELL')
            and   name  = 'HUMANOID_SPELL2_FK'
            and   indid > 0
            and   indid < 255)
   drop index HUMANOID_SPELL.HUMANOID_SPELL2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HUMANOID_SPELL')
            and   name  = 'HUMANOID_SPELL_FK'
            and   indid > 0
            and   indid < 255)
   drop index HUMANOID_SPELL.HUMANOID_SPELL_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('HUMANOID_SPELL')
            and   type = 'U')
   drop table HUMANOID_SPELL
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HUMANOID_WEAPON')
            and   name  = 'HUMANOID_WEAPON2_FK'
            and   indid > 0
            and   indid < 255)
   drop index HUMANOID_WEAPON.HUMANOID_WEAPON2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HUMANOID_WEAPON')
            and   name  = 'HUMANOID_WEAPON_FK'
            and   indid > 0
            and   indid < 255)
   drop index HUMANOID_WEAPON.HUMANOID_WEAPON_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('HUMANOID_WEAPON')
            and   type = 'U')
   drop table HUMANOID_WEAPON
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SPELL')
            and   type = 'U')
   drop table SPELL
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SURFACE')
            and   type = 'U')
   drop table SURFACE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SURFACERULE')
            and   type = 'U')
   drop table SURFACERULE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('WEAPON')
            and   type = 'U')
   drop table WEAPON
go

/*==============================================================*/
/* Table: ARMOR                                                 */
/*==============================================================*/
create table ARMOR (
   ID                   varchar(36)          not null,
   ARMORTYPE            varchar(20)          not null,
   PHYSICALDEF          int                  not null,
   MAGICALDEF           int                  not null,
   NAME                 varchar(40)          not null,
   constraint PK_ARMOR primary key (ID)
)
go

/*==============================================================*/
/* Table: HUMANOID                                              */
/*==============================================================*/
create table HUMANOID (
   ID                   varchar(36)          not null,
   HUMANOIDRACE         varchar(20)          not null,
   WEAPON               varchar(36)          null,
   ATTACKSPELL          varchar(36)          null,
   DEFENSIVESPELL       varchar(36)          null,
   HP                   int                  not null,
   MP                   int                  not null,
   STRENGTH             int                  not null,
   AGILITY              int                  not null,
   WISDOM               int                  not null,
   CONSTITUTION         int                  not null,
   NAME                 varchar(40)          not null,
   constraint PK_HUMANOID primary key (ID)
)
go

/*==============================================================*/
/* Table: HUMANOID_ARMOR                                        */
/*==============================================================*/
create table HUMANOID_ARMOR (
   ID                   varchar(36)          not null,
   ARM_ID               varchar(36)          not null,
   constraint PK_HUMANOID_ARMOR primary key (ID, ARM_ID)
)
go

/*==============================================================*/
/* Index: HUMANOID_ARMOR_FK                                     */
/*==============================================================*/
create index HUMANOID_ARMOR_FK on HUMANOID_ARMOR (
ID ASC
)
go

/*==============================================================*/
/* Index: HUMANOID_ARMOR2_FK                                    */
/*==============================================================*/
create index HUMANOID_ARMOR2_FK on HUMANOID_ARMOR (
ARM_ID ASC
)
go

/*==============================================================*/
/* Table: HUMANOID_SPELL                                        */
/*==============================================================*/
create table HUMANOID_SPELL (
   ID                   varchar(36)          not null,
   SPE_ID               varchar(36)          not null,
   constraint PK_HUMANOID_SPELL primary key (ID, SPE_ID)
)
go

/*==============================================================*/
/* Index: HUMANOID_SPELL_FK                                     */
/*==============================================================*/
create index HUMANOID_SPELL_FK on HUMANOID_SPELL (
ID ASC
)
go

/*==============================================================*/
/* Index: HUMANOID_SPELL2_FK                                    */
/*==============================================================*/
create index HUMANOID_SPELL2_FK on HUMANOID_SPELL (
SPE_ID ASC
)
go

/*==============================================================*/
/* Table: HUMANOID_WEAPON                                       */
/*==============================================================*/
create table HUMANOID_WEAPON (
   ID                   varchar(36)          not null,
   WEA_ID               varchar(36)          not null,
   constraint PK_HUMANOID_WEAPON primary key (ID, WEA_ID)
)
go

/*==============================================================*/
/* Index: HUMANOID_WEAPON_FK                                    */
/*==============================================================*/
create index HUMANOID_WEAPON_FK on HUMANOID_WEAPON (
ID ASC
)
go

/*==============================================================*/
/* Index: HUMANOID_WEAPON2_FK                                   */
/*==============================================================*/
create index HUMANOID_WEAPON2_FK on HUMANOID_WEAPON (
WEA_ID ASC
)
go

/*==============================================================*/
/* Table: SPELL                                                 */
/*==============================================================*/
create table SPELL (
   ID                   varchar(36)          not null,
   SPELLTYPE            varchar(20)          not null,
   SCHOOLSPELL          varchar(20)          not null,
   MAGICALPOWER         int                  not null,
   MANACOST             int                  not null,
   TOTALDURATION        int                  not null,
   NAME                 varchar(40)          not null,
   constraint PK_SPELL primary key (ID)
)
go

/*==============================================================*/
/* Table: SURFACE                                               */
/*==============================================================*/
create table SURFACE (
   CURRENTSURFACETYPE   varchar(40)          not null,
   HUMANOIDRACE         varchar(20)          not null,
   constraint PK_SURFACE primary key nonclustered (CURRENTSURFACETYPE)
)
go

/*==============================================================*/
/* Table: SURFACERULE                                           */
/*==============================================================*/
create table SURFACERULE (
   CURRENTSURFACETYPE   varchar(40)          not null,
   HUMANOIDRACE         varchar(20)          not null,
   HP                   int                  not null,
   MP                   int                  not null,
   STRENGTH             int                  not null,
   WISDOM               int                  not null,
   AGILITY              int                  not null,
   CONSTITUTION         int                  not null,
   constraint PK_SURFACERULE primary key (CURRENTSURFACETYPE)
)
go

/*==============================================================*/
/* Table: WEAPON                                                */
/*==============================================================*/
create table WEAPON (
   ID                   varchar(36)          not null,
   WEAPONTYPE           varchar(20)          not null,
   PHYSICALATTACK       int                  not null,
   MAGICALATTACK        int                  not null,
   NAME                 varchar(40)          not null,
   constraint PK_WEAPON primary key (ID)
)
go

