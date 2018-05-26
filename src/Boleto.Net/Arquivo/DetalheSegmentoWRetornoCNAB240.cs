using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class DetalheSegmentoWRetornoCNAB240
    {
        #region Variáveis

        int _codigoErro = 0;

        #endregion

        #region Construtores

        public DetalheSegmentoWRetornoCNAB240()
        {
        }

        #endregion

        #region Propriedades

        public int CodigoErro
        {
            get { return _codigoErro; }
            set { _codigoErro = value; }
        }

        #endregion

        #region Métodos de Instância

        public void LerDetalheSegmentoWRetornoCNAB240(string registro)
        {
            throw new Exception(
                "Esse arquivo de retorno contém um segmento W, " +
                "que se trata de uma pré-crítica. " +
                "Abra um chamado para o DTI anexando " +
                "este arquivo de retorno e as últimas remessas enviadas para ser feita uma análise!");
        }

        #endregion
    }
}
