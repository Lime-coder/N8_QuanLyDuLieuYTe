@echo off
chcp 65001
set NLS_LANG=AMERICAN_AMERICA.AL32UTF8
sqlplus -s hospital_dba/123@localhost:1521/PDB_QLYT @check.sql
