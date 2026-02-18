-- =====================================================
-- SCRIPT COMPLETO - BANCO DE DADOS RIDEFLOW
-- =====================================================

-- DROP DO BANCO (opcional - CUIDADO!)
-- DROP DATABASE IF EXISTS rideflow;
-- CREATE DATABASE rideflow;
-- \c rideflow;

-- =====================================================
-- 1. DROP DAS TABELAS (se existirem) - ordem correta
-- =====================================================
DROP TABLE IF EXISTS tb_rating CASCADE;
DROP TABLE IF EXISTS tb_ride CASCADE;
DROP TABLE IF EXISTS driver_servicetype CASCADE;
DROP TABLE IF EXISTS tb_driver CASCADE;
DROP TABLE IF EXISTS tb_servicetype CASCADE;
DROP TABLE IF EXISTS tb_user CASCADE;

-- =====================================================
-- 2. CRIAÇÃO DAS TABELAS
-- =====================================================

-- Tabela de usuários
CREATE TABLE tb_user (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nameUser VARCHAR(256) NOT NULL,
    cpf VARCHAR(11) UNIQUE NOT NULL,
    phoneUser VARCHAR(13),
    created_at TIMESTAMPTZ DEFAULT NOW()
);

-- Tabela de motoristas
CREATE TABLE tb_driver (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nameDriver VARCHAR(256) NOT NULL,
    cnh VARCHAR(9) UNIQUE NOT NULL,
    plate VARCHAR(7) UNIQUE NOT NULL,
    yearCar INT NOT NULL,
    modelCar VARCHAR(256) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW()    
);

-- Tabela de tipos de serviço (categorias)
CREATE TABLE tb_servicetype (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    category VARCHAR(50) NOT NULL -- 'basic', 'premium', 'vip'
);

-- Tabela de relacionamento motorista x serviço
CREATE TABLE driver_servicetype (
    driver_id UUID REFERENCES tb_driver(id) ON DELETE CASCADE,
    servicetype_id UUID REFERENCES tb_servicetype(id) ON DELETE CASCADE,
    PRIMARY KEY (driver_id, servicetype_id)
);

-- Tabela de corridas
CREATE TABLE tb_ride (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    startPoint VARCHAR(256) NOT NULL,
    destiny VARCHAR(256) NOT NULL,
    ride_status VARCHAR(50) NOT NULL DEFAULT 'in_progress', -- 'in_progress', 'finished', 'canceled'
    user_id UUID NOT NULL,
    driver_id UUID NOT NULL,
    servicetype_id UUID NOT NULL,
    distance_km DECIMAL NOT NULL DEFAULT 0,
    total_value DECIMAL NOT NULL DEFAULT 0,
    payment_method VARCHAR(50) NOT NULL, -- 'pix', 'credit_card', 'debit_card'
    created_at TIMESTAMPTZ DEFAULT NOW(),
    
    CONSTRAINT fk_ride_driver FOREIGN KEY(driver_id) REFERENCES tb_driver(id),
    CONSTRAINT fk_ride_servicetype FOREIGN KEY(servicetype_id) REFERENCES tb_servicetype(id),
    CONSTRAINT fk_ride_user FOREIGN KEY(user_id) REFERENCES tb_user(id)  
);

-- Tabela de avaliações
CREATE TABLE tb_rating (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    rate INT NOT NULL CHECK (rate >= 1 AND rate <= 5),
    comment VARCHAR(256),
    ride_id UUID NOT NULL,
    CONSTRAINT fk_rating_ride FOREIGN KEY(ride_id) REFERENCES tb_ride(id)
);

-- =====================================================
-- 3. INSERÇÃO DE DADOS INICIAIS
-- =====================================================

-- Inserir categorias de serviço
INSERT INTO tb_servicetype (category) VALUES 
    ('basic'),
    ('premium'),
    ('vip');

-- Inserir usuários
INSERT INTO tb_user (nameUser, cpf, phoneUser) VALUES
    ('Ana Silva', '12345678901', '11999990001'),
    ('Bruno Costa', '12345678902', '11999990002');

-- Inserir motoristas 
INSERT INTO tb_driver (nameDriver, cnh, plate, yearCar, modelCar) VALUES
    ('Carlos Motorista', 'CNH00001', 'ABC1D23', 2013, 'Toyota Corolla'),
    ('Daniel Motorista', 'CNH00002', 'XYZ9K88', 2022, 'Honda Civic');

-- Relacionar motoristas com categorias baseado no ano do carro
-- Carlos (2013) -> basic
INSERT INTO driver_servicetype (driver_id, servicetype_id)
SELECT d.id, s.id 
FROM tb_driver d, tb_servicetype s 
WHERE d.nameDriver = 'Carlos Motorista' AND s.category = 'basic';

-- Daniel (2022) -> vip
INSERT INTO driver_servicetype (driver_id, servicetype_id)
SELECT d.id, s.id 
FROM tb_driver d, tb_servicetype s 
WHERE d.nameDriver = 'Daniel Motorista' AND s.category = 'vip';

-- Inserir corridas
-- Corrida 1: Ana Silva com Carlos (basic, pix)
INSERT INTO tb_ride (
    startPoint, destiny, ride_status, user_id, driver_id, 
    servicetype_id, distance_km, total_value, payment_method, created_at
)
SELECT
    'Av. Paulista, 1000',
    'Rua Augusta, 200',
    'finished',
    u.id,
    d.id,
    s.id,
    15.5,
    45.90,
    'pix',
    NOW() - INTERVAL '2 days'
FROM tb_user u, tb_driver d, tb_servicetype s
WHERE u.nameUser = 'Ana Silva' 
  AND d.nameDriver = 'Carlos Motorista'
  AND s.category = 'basic';

-- Corrida 2: Bruno Costa com Daniel (vip, credit_card)
INSERT INTO tb_ride (
    startPoint, destiny, ride_status, user_id, driver_id, 
    servicetype_id, distance_km, total_value, payment_method, created_at
)
SELECT
    'Shopping Center',
    'Aeroporto',
    'finished',
    u.id,
    d.id,
    s.id,
    25.0,
    120.50,
    'credit_card',
    NOW() - INTERVAL '1 day'
FROM tb_user u, tb_driver d, tb_servicetype s
WHERE u.nameUser = 'Bruno Costa' 
  AND d.nameDriver = 'Daniel Motorista'
  AND s.category = 'vip';

-- Corrida 3: Em andamento (para testes)
INSERT INTO tb_ride (
    startPoint, destiny, ride_status, user_id, driver_id, 
    servicetype_id, distance_km, total_value, payment_method, created_at
)
SELECT
    'Centro',
    'Universidade',
    'in_progress',
    u.id,
    d.id,
    s.id,
    8.0,
    25.90,
    'debit_card',
    NOW()
FROM tb_user u, tb_driver d, tb_servicetype s
WHERE u.nameUser = 'Ana Silva' 
  AND d.nameDriver = 'Daniel Motorista'
  AND s.category = 'vip';

-- Inserir avaliaÃ§Ãµes para as corridas finalizadas
INSERT INTO tb_rating (rate, comment, ride_id)
SELECT
    5,
    'Corrida excelente, motorista muito educado',
    r.id
FROM tb_ride r
WHERE r.ride_status = 'finished'
LIMIT 1;

INSERT INTO tb_rating (rate, comment, ride_id)
SELECT
    4,
    'Boa viagem, mas trÃ¢nsito pesado',
    r.id
FROM tb_ride r
WHERE r.ride_status = 'finished' 
  AND r.payment_method = 'credit_card'
LIMIT 1;

-- =====================================================
-- 4. CONSULTAS DE VERIFICAÇÃO
-- =====================================================

-- Verificar todas as tabelas
SELECT 'tb_user' AS tabela, COUNT(*) FROM tb_user
UNION ALL
SELECT 'tb_driver', COUNT(*) FROM tb_driver
UNION ALL
SELECT 'tb_servicetype', COUNT(*) FROM tb_servicetype
UNION ALL
SELECT 'driver_servicetype', COUNT(*) FROM driver_servicetype
UNION ALL
SELECT 'tb_ride', COUNT(*) FROM tb_ride
UNION ALL
SELECT 'tb_rating', COUNT(*) FROM tb_rating;

-- Verificar corridas com joins completos
SELECT 
    r.id,
    u.nameUser AS passageiro,
    d.nameDriver AS motorista,
    d.plate,
    s.category AS categoria,
    r.ride_status AS status,
    r.total_value AS valor,
    r.payment_method AS pagamento,
    r.startPoint AS origem,
    r.destiny AS destino,
    r.created_at AS data
FROM tb_ride r
JOIN tb_user u ON r.user_id = u.id
JOIN tb_driver d ON r.driver_id = d.id
JOIN tb_servicetype s ON r.servicetype_id = s.id
ORDER BY r.created_at DESC;

-- Verificar motoristas VIP (ano >= 2020)
SELECT *
FROM tb_driver
WHERE yearCar >= 2020;

-- Verificar avaliações
SELECT 
    u.nameUser AS passageiro,
    d.nameDriver AS motorista,
    r.ride_status AS status,
    rt.rate AS nota,
    rt.comment AS comentario
FROM tb_rating rt
JOIN tb_ride r ON rt.ride_id = r.id
JOIN tb_user u ON r.user_id = u.id
JOIN tb_driver d ON r.driver_id = d.id;