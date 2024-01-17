-- Criação das colunas necessárias;

ALTER TABLE relatorio ADD COLUMN recurso_novo TEXT DEFAULT NULL;
ALTER TABLE relatorio ADD COLUMN abreviacao TEXT DEFAULT NULL;
ALTER TABLE relatorio ADD COLUMN identificador TEXT DEFAULT NULL;

-- Atualização das colunas com os valores temporários;

UPDATE relatorio SET recurso_novo = REGEXP_REPLACE(recurso, E'\\x2013', '-', 'g');
UPDATE relatorio SET identificador = dia || recurso_novo;

-- Atualização das colunas com os valores definitivos;

UPDATE relatorio SET identificador = REGEXP_REPLACE(identificador, '([0-9]{4})-([0-9]{2})-([0-9]{2})([A-Z]{3,}) - ([C|R]{1})?([a-z]+)?( - )?([A-z]+) ([0-9]{3})(\s{1,})?(\(.*\))?', '\1\2\3\4\5\9', 'g');
UPDATE relatorio SET abreviacao = REGEXP_REPLACE(recurso_novo, '([A-Z]{3,}) - ([C|R]{1})?([a-z]+)?( - )?([A-z]+) ([0-9]{3})(\s{1,})?(\(.*\))?', '\1\2\6', 'g');

-- Verificação se os valores estão corretos;

SELECT dia, recurso, recurso_novo, abreviacao, identificador, ENCODE(recurso::bytea, 'HEX') as hexadecimal FROM relatorio LIMIT 10 OFFSET 0;

-- Substituição das colunas atualizadas e descarte das colunas temporárias;

ALTER TABLE relatorio DROP COLUMN recurso;
ALTER TABLE relatorio RENAME COLUMN recurso_novo TO recurso;

-- Recriar o índice excluído para agilizar as alterações na coluna;

CREATE UNIQUE INDEX "IX_relatorio_identificador" ON public.relatorio USING btree ('serial');
CREATE UNIQUE INDEX "IX_composicao_identificador" ON public.composicao USING btree (identificador);
