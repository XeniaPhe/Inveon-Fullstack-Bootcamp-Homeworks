namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.ISP.Bad;

/*
 * Why does this interface violate the Interface Segregation Principle?
 * 
 * Because the IEspressoMachine interface forces all implementing classes to define methods that they might not need. For example:
 * 
 * Basic espresso machines may not support milk based espresso drinks such as latte, cappuccino and cortado so they don't need to
 * implement all of these methods.
 */
internal interface IEspressoMachine
{
    void MakeEspresso();
    void MakeRistretto();
    void MakeCortado();
    void MakeCappuccino();
    void MakeLatte();
    void MakeAmericano();
    void MakeHotWater();
    void CleanMachine();
    void RefillBeans();
    void RefillWater();
}