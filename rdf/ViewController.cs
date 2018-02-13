using System;
using System.Collections.Generic;

using AppKit;
using Foundation;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace rdf
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private HashSet<String> subjectMas = new HashSet<string>();
        private HashSet<String> objectMas = new HashSet<string>();
        private HashSet<String> predicateMas = new HashSet<string>();

        private Notation3Parser parser = new Notation3Parser();
        private Graph graph = new Graph();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            subjectMas.Add("all");
            objectMas.Add("all");
            predicateMas.Add("all");

            parser.Load(graph, @"/Users/Kopytov/Documents/Programming/rdf/rdf/rdf.owl");
            //Do any additional setup after loading the view.

            foreach (Triple triple in graph.Triples)
            {
                string s = GetNodeString(triple.Subject);

                subjectMas.Add(returnPrefix(GetNodeString(triple.Subject)));
                objectMas.Add(returnPrefix(GetNodeString(triple.Object)));
                predicateMas.Add(returnPrefix(GetNodeString(triple.Predicate)));
                }

            String[] temp1 = new String[subjectMas.Count];
            subjectMas.CopyTo(temp1);
            String[] temp2 = new String[objectMas.Count];
            objectMas.CopyTo(temp2);
            String[] temp3 = new String[predicateMas.Count];
            predicateMas.CopyTo(temp3);

            subjectMenu.RemoveAllItems();
            objectMenu.RemoveAllItems();
            predicateMenu.RemoveAllItems();

            subjectMenu.AddItems(temp1);
            objectMenu.AddItems(temp2);
            predicateMenu.AddItems(temp3);

        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }


        partial void showAll(NSObject sender)
        {
            //TODO: showAll
            String str1 = subjectMenu.SelectedItem.Title;
            String str2 = predicateMenu.SelectedItem.Title;
            String str3 = objectMenu.SelectedItem.Title;
            String str4 = @"";

            bool[] myBoolean = new bool[3];

            if (str1 == @"all") {
                str1 = "?x";
                str4 += str1+" ";
                myBoolean[0] = false;
            } else{
                //str1 = ":" + str1;
                myBoolean[0] = true;
            }
            if (str2 == @"all") {
                str2 = "?y";
                str4 += str2+" ";
                myBoolean[1] = false;
            } else{
                //str2 = ":" + str2;
                myBoolean[1] = true;
            }
            if (str3 == @"all") { 
                str3 = "?z";
                str4 += str3+" ";
                myBoolean[2] = false;
            } else{
                //str3 = ":" + str3;
                myBoolean[2] = true;
            }

            if (myBoolean[0] && myBoolean[1] && myBoolean[2]) str4 = "*";

            string mySparql = @"
    PREFIX : <http://kopytov.rdf#>
    PREFIX ns0: <http://our-place.spb.ru/today#>
    PREFIX rdfs:<http://www.w3.org/2000/01/rdf-schema#>
    PREFIX owl: <http://www.w3.org/2002/07/owl#>
    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
    
    SELECT "+str4+@"
    WHERE 
    {
        "+str1+@" "+str2+@" "+str3+@".                                   

    }";
            

            SparqlResultSet resultSet = graph.ExecuteQuery(mySparql) as SparqlResultSet;
            if (resultSet != null)
            {
                textBox.StringValue = "";
                textBox.StringValue += "####################\nOutput:\n####################\n";
                for (int i = 0; i < resultSet.Count; i++)
                {
                    SparqlResult result = resultSet[i];
                    textBox.StringValue += string.Format("{0}. {1} {2} {3}\n", i + 1, myBoolean[0]? str1 : returnPrefix(GetNodeString(result["x"])), myBoolean[1]? str2 : returnPrefix(GetNodeString(result["y"])), myBoolean[2]? str3 : returnPrefix(GetNodeString(result["z"])));
                }
            } else{
                textBox.StringValue = "No triples found";
            }

        }


        partial void showInstances(NSObject sender)
        {
            const string mySparql = @"
    PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
    PREFIX ns0: <http://our-place.spb.ru/today#>
    PREFIX rdfs:<http://www.w3.org/2000/01/rdf-schema#Resource/>
    PREFIX owl: <http://www.w3.org/2002/07/owl#>
    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
    
    SELECT ?x
    WHERE 
    {
        ?x rdf:type owl:NamedIndividual.
    }";
            
            textBox.StringValue = "";
            textBox.StringValue += "####################\nInstances:\n####################\n";
            funcOne(mySparql);

        }


        partial void showProperties(NSObject sender)
        {
            const string mySparql = @"
    PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
    PREFIX ns0: <http://our-place.spb.ru/today#>
    PREFIX rdfs:<http://www.w3.org/2000/01/rdf-schema#Resource/>
    PREFIX owl: <http://www.w3.org/2002/07/owl#>
    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
    
    SELECT ?x
    WHERE 
    {
        ?x rdf:type owl:ObjectProperty.
    }";
            const string mySparql2 = @"
    PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
    PREFIX ns0: <http://our-place.spb.ru/today#>
    PREFIX rdfs:<http://www.w3.org/2000/01/rdf-schema#Resource/>
    PREFIX owl: <http://www.w3.org/2002/07/owl#>
    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
    
    SELECT ?x
    WHERE 
    {
        ?x rdf:type owl:DatatypeProperty.
    }";
            textBox.StringValue = "";
            textBox.StringValue += "####################\nObject Properties:\n####################\n";
            funcOne(mySparql);
            textBox.StringValue += "\n####################\nData Properties:\n####################\n";
            funcOne(mySparql2);
        }

        partial void showObjects(NSObject sender)
        {
            const string mySparql = @"
    PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
    PREFIX ns0: <http://our-place.spb.ru/today#>
    PREFIX rdfs:<http://www.w3.org/2000/01/rdf-schema#Resource/>
    PREFIX owl: <http://www.w3.org/2002/07/owl#>
    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
    
    SELECT ?x
    WHERE 
    {
        ?x rdf:type owl:Class.
    }";
            textBox.StringValue = "";
            textBox.StringValue += "####################\nClasses:\n####################\n";
            funcOne(mySparql);
        }
        //начало

        void funcOne(string getInsomnia)
        {

            SparqlResultSet resultSet = graph.ExecuteQuery(getInsomnia) as SparqlResultSet;
            if (resultSet != null)
            {
                for (int i = 0; i < resultSet.Count; i++)
                {
                            SparqlResult result = resultSet[i];
                            textBox.StringValue += string.Format("{0}. {1}\n", i + 1, GetNodeString(result["x"]));
                }
            }
        }

        private string returnPrefix(string str){
            string smth = "";
            string owlPrefix = "owl:";
            string rdfPrefix = "rdf:";
            string rdfsPrefix = "rdfs:";
            string myPrefix = ":";
            switch (str){
                case "ObjectProperty":
                    smth = owlPrefix + str;
                    break;
                case "DatatypeProperty":
                    smth = owlPrefix + str;
                    break;
                case "Class":
                    smth = owlPrefix + str;
                    break;
                case "":
                    smth = owlPrefix + str;
                    break;
                case "NamedIndividual":
                    smth = owlPrefix + str;
                    break;
                case "type":
                    smth = rdfPrefix + str;
                    break;
                case "subClassOf":
                    smth = rdfsPrefix + str;
                    break;
                default:
                    smth = myPrefix + str;
                    break;
            }
            return smth;
        }

        string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    int lik = s.LastIndexOf('/');
                    if (lio == -1)
                    {
                        if (lik == -1)
                        {
                            return s;
                        }
                        else
                            return s.Substring(lik + 1);
                    }
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }

        //конец
    }
}
