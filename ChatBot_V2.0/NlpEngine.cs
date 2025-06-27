/*


using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.SentenceDetect;
using System.IO;

public class NlpEngine
{
    private EnglishMaximumEntropyTokenizer tokenizer;
    private EnglishMaximumEntropyPosTagger posTagger;
    private EnglishMaximumEntropySentenceDetector sentenceDetector;

    public NlpEngine()
    {
        string modelsPath = Path.Combine(Directory.GetCurrentDirectory(), "Models");

        tokenizer = new EnglishMaximumEntropyTokenizer(Path.Combine(modelsPath, "en-token.bin"));
        posTagger = new EnglishMaximumEntropyPosTagger(
            Path.Combine(modelsPath, "en-pos-maxent.bin"),
            Path.Combine(modelsPath, "en-pos-maxent.dict")); // optional dictionary file
        sentenceDetector = new EnglishMaximumEntropySentenceDetector(Path.Combine(modelsPath, "en-sent.bin"));
    }

    public string[] Tokenize(string input) => tokenizer.Tokenize(input);

    public string[] Tag(string[] tokens) => posTagger.Tag(tokens);

    public string[] DetectSentences(string input) => sentenceDetector.SentenceDetect(input);
}
*/