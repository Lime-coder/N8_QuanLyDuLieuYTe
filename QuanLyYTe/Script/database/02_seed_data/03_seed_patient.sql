-- ==============================================================================
-- 03_seed_patient.sql
-- Ch?y du?i quy?n: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;
SET SQLBLANKLINES ON;

-- ============================================================
-- AUTO-GENERATE 100.000 BỆNH NHÂN
-- ============================================================
INSERT /*+ APPEND */ INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db)
SELECT
    'BN' || LPAD(LEVEL, 6, '0') AS patient_id,
    CASE 
        WHEN MOD(LEVEL, 2) = 0 THEN
            CASE MOD(LEVEL, 14)
                WHEN 0 THEN UNISTR('Nguy\1EC5n Th\1ECB Thu H\00E0')
                WHEN 1 THEN UNISTR('Tr\1EA7n Th\1ECB Mai')
                WHEN 2 THEN UNISTR('L\00EA Ng\1ECDc Anh')
                WHEN 3 THEN UNISTR('Ph\1EA1m Th\00F9y Linh')
                WHEN 4 THEN UNISTR('Ho\00E0ng M\1EF9 Duy\00EAn')
                WHEN 5 THEN UNISTR('V\00F5 Thanh Tr\00FAc')
                WHEN 6 THEN UNISTR('\0110\1EB7ng H\00E0 My')
                WHEN 7 THEN UNISTR('B\00F9i Minh Ch\00E2u')
                WHEN 8 THEN UNISTR('\0110\1ED7 Th\1EA3o Vy')
                WHEN 9 THEN UNISTR('Phan Ng\1ECDc H\00E2n')
                WHEN 10 THEN UNISTR('\0110inh Thu Ph\01B0\01A1ng')
                WHEN 11 THEN UNISTR('Cao B\1EA3o Ng\1ECDc')
                WHEN 12 THEN UNISTR('Nguy\1EC5n Kh\00E1nh Linh')
                WHEN 13 THEN UNISTR('Tr\1EA7n Gia H\00E2n')
                ELSE UNISTR('L\00EA Minh Th\01B0')
            END
        ELSE
            CASE MOD(LEVEL, 15)
                WHEN 0 THEN UNISTR('Nguy\1EC5n V\0103n An')
                WHEN 1 THEN UNISTR('Tr\1EA7n Minh Qu\00E2n')
                WHEN 2 THEN UNISTR('L\00EA Qu\1ED1c Huy')
                WHEN 3 THEN UNISTR('Ph\1EA1m Ho\00E0ng Nam')
                WHEN 4 THEN UNISTR('Ho\00E0ng \0110\1EE9c D\0169ng')
                WHEN 5 THEN UNISTR('V\00F5 Thanh T\00F9ng')
                WHEN 6 THEN UNISTR('\0110\1EB7ng Gia B\1EA3o')
                WHEN 7 THEN UNISTR('B\00F9i Anh Tu\1EA5n')
                WHEN 8 THEN UNISTR('\0110\1ED7 Minh Khang')
                WHEN 9 THEN UNISTR('Phan Th\00E0nh \0110\1EA1t')
                WHEN 10 THEN UNISTR('\0110inh Nh\1EADt Minh')
                WHEN 11 THEN UNISTR('Cao Gia Huy')
                WHEN 12 THEN UNISTR('Nguy\1EC5n H\1EA3i Nam')
                WHEN 13 THEN UNISTR('Tr\1EA7n Qu\1ED1c B\1EA3o')
                ELSE UNISTR('L\00EA \0110\1EE9c Anh')
            END
    END AS full_name,
    CASE
        WHEN MOD(LEVEL, 2) = 0 THEN UNISTR('N\1EEF')
        ELSE UNISTR('Nam')
    END AS gender,
    DATE '1950-01-01' + MOD(LEVEL * 37, 20000) AS birthdate,
    '001080' || LPAD(LEVEL, 6, '0') AS id_card,
    TO_CHAR(MOD(LEVEL, 300) + 1) AS house_no,
    CASE MOD(LEVEL, 10)
        WHEN 0 THEN UNISTR('Nguy\1EC5n Tr\00E3i')
        WHEN 1 THEN UNISTR('L\00EA L\1EE3i')
        WHEN 2 THEN UNISTR('Tr\1EA7n H\01B0ng \0110\1EA1o')
        WHEN 3 THEN UNISTR('C\00E1ch M\1EA1ng Th\00E1ng T\00E1m')
        WHEN 4 THEN UNISTR('\0110i\1EC7n Bi\00EAn Ph\1EE7')
        WHEN 5 THEN UNISTR('V\00F5 V\0103n Ki\1EC7t')
        WHEN 6 THEN UNISTR('Ph\1EA1m V\0103n \0110\1ED3ng')
        WHEN 7 THEN UNISTR('Nguy\1EC5n V\0103n C\1EEB')
        WHEN 8 THEN UNISTR('Ho\00E0ng V\0103n Th\1EE5')
        ELSE UNISTR('L\00FD Th\01B0\1EDDng Ki\1EC7t')
    END AS street,
    UNISTR('Qu\1EADn ') || TO_CHAR(MOD(LEVEL, 12) + 1) AS district,
    CASE MOD(LEVEL, 5)
        WHEN 0 THEN UNISTR('TP. H\1ED3 Ch\00ED Minh')
        WHEN 1 THEN UNISTR('H\00E0 N\1ED9i')
        WHEN 2 THEN UNISTR('H\1EA3i Ph\00F2ng')
        WHEN 3 THEN UNISTR('C\1EA7n Th\01A1')
        ELSE UNISTR('\0110\00E0 N\1EB5ng')
    END AS city_province,
    CASE MOD(LEVEL, 10)
        WHEN 0 THEN UNISTR('T\0103ng huy\1EBFt \00E1p nhi\1EC1u n\0103m, \0111ang d\00F9ng thu\1ED1c h\1EA1 \00E1p \0111\1ECBnh k\1EF3.')
        WHEN 1 THEN UNISTR('Vi\00EAm d\1EA1 d\00E0y m\1EA1n t\00EDnh, hay \0111au v\00F9ng th\01B0\1EE3ng v\1ECB sau \0103n.')
        WHEN 2 THEN UNISTR('\0110\00E1i th\00E1o \0111\01B0\1EDDng type 2, \0111ang theo d\00F5i \0111\01B0\1EDDng huy\1EBFt \0111\1ECBnh k\1EF3.')
        WHEN 3 THEN UNISTR('R\1ED1i lo\1EA1n lipid m\00E1u, \0111\01B0\1EE3c khuy\00EAn \0111i\1EC1u ch\1EC9nh ch\1EBF \0111\1ED9 \0103n.')
        WHEN 4 THEN UNISTR('Vi\00EAm xoang d\1ECB \1EE9ng, th\01B0\1EDDng t\00E1i ph\00E1t khi thay \0111\1ED5i th\1EDDi ti\1EBFt.')
        WHEN 5 THEN UNISTR('T\1EEBng ph\1EABu thu\1EADt ru\1ED9t th\1EEBa, hi\1EC7n kh\00F4ng ghi nh\1EADn bi\1EBFn ch\1EE9ng.')
        WHEN 6 THEN UNISTR('\0110au n\1EEDa \0111\1EA7u t\00E1i ph\00E1t, t\0103ng khi c\0103ng th\1EB3ng ho\1EB7c thi\1EBFu ng\1EE7.')
        WHEN 7 THEN UNISTR('C\00F3 ti\1EC1n s\1EED hen ph\1EBF qu\1EA3n nh\1EB9, \00EDt khi l\00EAn c\01A1n c\1EA5p.')
        WHEN 8 THEN UNISTR('T\1EEBng nh\1EADp vi\1EC7n do vi\00EAm ph\1ED5i, \0111\00E3 \0111i\1EC1u tr\1ECB \1ED5n \0111\1ECBnh.')
        ELSE UNISTR('Ch\01B0a ghi nh\1EADn b\1EC7nh n\1EC1n \0111\00E1ng ch\00FA \00FD.')
    END AS medical_history,
    CASE MOD(LEVEL, 8)
        WHEN 0 THEN UNISTR('B\1ED1 b\1ECB t\0103ng huy\1EBFt \00E1p, \0111ang \0111i\1EC1u tr\1ECB ngo\1EA1i tr\00FA.')
        WHEN 1 THEN UNISTR('M\1EB9 b\1ECB \0111\00E1i th\00E1o \0111\01B0\1EDDng type 2.')
        WHEN 2 THEN UNISTR('Gia \0111\00ECnh c\00F3 ti\1EC1n s\1EED b\1EC7nh tim m\1EA1ch.')
        WHEN 3 THEN UNISTR('Anh/ch\1ECB/em ru\1ED9t t\1EEBng b\1ECB vi\00EAm d\1EA1 d\00E0y m\1EA1n.')
        WHEN 4 THEN UNISTR('\00D4ng/b\00E0 c\00F3 ti\1EC1n s\1EED tai bi\1EBFn m\1EA1ch m\00E1u n\00E3o.')
        WHEN 5 THEN UNISTR('Gia \0111\00ECnh c\00F3 ng\01B0\1EDDi m\1EAFc r\1ED1i lo\1EA1n lipid m\00E1u.')
        WHEN 6 THEN UNISTR('Kh\00F4ng ghi nh\1EADn b\1EC7nh di truy\1EC1n ho\1EB7c b\1EC7nh m\1EA1n t\00EDnh trong gia \0111\00ECnh.')
        ELSE UNISTR('Ch\01B0a khai th\00E1c \0111\1EA7y \0111\1EE7 ti\1EC1n s\1EED b\1EC7nh gia \0111\00ECnh.')
    END AS family_medical_history,
    CASE MOD(LEVEL, 8)
        WHEN 0 THEN UNISTR('D\1ECB \1EE9ng Penicillin, t\1EEBng n\1ED5i m\1EA9n \0111\1ECF sau khi s\1EED d\1EE5ng.')
        WHEN 1 THEN UNISTR('D\1ECB \1EE9ng h\1EA3i s\1EA3n, bi\1EC3u hi\1EC7n ng\1EE9a v\00E0 n\1ED5i m\1EC1 \0111ay.')
        WHEN 2 THEN UNISTR('D\1ECB \1EE9ng thu\1ED1c gi\1EA3m \0111au nh\00F3m NSAID.')
        WHEN 3 THEN UNISTR('D\1ECB \1EE9ng ph\1EA5n hoa, th\01B0\1EDDng h\1EAFt h\01A1i v\00E0 ngh\1EB9t m\0169i.')
        WHEN 4 THEN UNISTR('D\1ECB \1EE9ng b\1EE5i nh\00E0, th\01B0\1EDDng g\00E2y vi\00EAm m\0169i d\1ECB \1EE9ng.')
        WHEN 5 THEN UNISTR('T\1EEBng d\1ECB \1EE9ng nh\1EB9 v\1EDBi thu\1ED1c kh\00E1ng sinh kh\00F4ng r\00F5 lo\1EA1i.')
        WHEN 6 THEN UNISTR('Kh\00F4ng ghi nh\1EADn d\1ECB \1EE9ng thu\1ED1c.')
        ELSE UNISTR('Ch\01B0a ghi nh\1EADn d\1ECB \1EE9ng.')
    END AS drug_allergies,
    'BN' || LPAD(LEVEL, 6, '0') AS username_db
FROM DUAL
CONNECT BY LEVEL <= 100000;

COMMIT;
SET SQLBLANKLINES OFF;
