CREATE TABLE IF NOT EXIST eletricista
(
  matricula INT NOT NULL,
  nome_completo VARCHAR(64) UNIQUE,
  CONSTRAINT PK_eletricista PRIMARY KEY (matricula)
);
CREATE TABLE IF NOT EXIST supervisor
(
  matricula INT NOT NULL,
  nome_completo VARCHAR(64) UNIQUE,
  CONSTRAINT PK_supervisor PRIMARY KEY (matricula)
);
CREATE TABLE IF NOT EXIST atividade
(
  id INT NOT NULL,
  atividade VARCHAR(16) UNIQUE,
  CONSTRAINT PK_atividade PRIMARY KEY (id)
);
CREATE TABLE IF NOT EXIST regional
(
  id INT NOT NULL,
  regional VARCHAR(16) UNIQUE,
  CONSTRAINT PK_regional PRIMARY KEY (id)
);
CREATE TABLE IF NOT EXIST composicao
(
  dia DATE NOT NULL,
  adesivo INT NOT NULL,
  placa CHAR(7) NOT NULL,
  recurso VARCHAR(32) NOT NULL,
  atividade INT NOT NULL,
  motorista INT NOT NULL,
  ajudante INT NOT NULL,
  supervisor INT NOT NULL,
  regional INT NOT NULL,
  CONSTRAINT PK_composicao PRIMARY KEY (dia,recurso),
  CONSTRAINT FK_motorista FOREIGN KEY (motorista) REFERENCES eletricista(matricula),
  CONSTRAINT FK_ajudante FOREIGN KEY (ajudante) REFERENCES eletricista(matricula),
  CONSTRAINT FK_supervisor FOREIGN KEY (supervisor) REFERENCES supervisor(matricula),
  CONSTRAINT FK_regional FOREIGN KEY (regional) REFERENCES regional(id),
  CONSTRAINT FK_atividade FOREIGN KEY (atividade) REFERENCES atividade(id)
);
